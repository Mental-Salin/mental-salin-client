using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using UnityHTTP;
using SalinSDK;
using SalinSDK.Pattern;
using System.IO;

public class Transmitter : Singleton<Transmitter>
{
    /// <summary>
    /// 프로토콜 요청 담아둘 이벤트 Q방식으로 처리합니다.
    /// 나중에 QueueManager를 따로만들어서 관리 할 수도 있습니다.
    /// </summary>
    private Queue<RequestData> reqQ;

    /// <summary>
    /// 통신중인 데이터가 있는지 체크합니다.
    /// </summary>
    public bool isTransmitData { get; private set; }
	
	// http 통신 방식입니다.
	private HTTPMethod method;

    /// <summary>
    /// 어느 용도로 사용 되었는지에 대한 타입입니다.
    /// </summary>
    private RequestDataType type;


    // 요청할 url입니다.
    private string reqStr;
	
	// 통신시 추가할 필드데이터입니다.
	private Dictionary<string, string> fieldDatas;
	
	// 통신시 추가할 헤더데이터입니다.
	private Dictionary<string, string> headerDatas;


    private bool needBlock;
	
	public override void Awake()
	{
		base.Awake();
		isTransmitData = false;
        reqQ = new Queue<RequestData>();
    }

	/// <summary>
	/// 통신에 필요한 데이터를 초기화합니다.
	/// </summary>
	private void ClearData()
	{
		isTransmitData 	= false;
		method 			= HTTPMethod.NONE;
        reqStr 			= string.Empty;
        type            = RequestDataType.NONE;
        fieldDatas 		= null;
		headerDatas 	= null;
        needBlock = true;
	}

	/// <summary>
	/// HTTPMethod 를 분석하여 Request를 생성합니다.
	/// </summary>
	/// <returns> 생성된 Request 입니다. </returns>
	private Request CreateCurrentReq()
	{
		Request req = null;

		// HTTPMethod 테크
		switch (method)
		{
			// POST, PUT, DELETE 통신의 경우 url을 유지 하고 리퀘스트 생성시 필드데이터를 추가하여 생성합니다. 
			case HTTPMethod.POST:			
				req = new Request(method.ToString(), reqStr, GetFieldTable());			
				break;
			case HTTPMethod.PUT:
				req = new Request(method.ToString(), reqStr, GetFieldTable());
				break;
			case HTTPMethod.DELETE:
				req = new Request(method.ToString(), reqStr, GetFieldTable());
				break;					
			
			// GET 통신시 url에 파라미터를 조합하여 리퀘스트를 생성합니다.
			case HTTPMethod.GET:
				StringBuilder reqStringBuilder = new StringBuilder(reqStr);
                if (fieldDatas.Count != 0)
                {
                    reqStringBuilder.Append("?");
                    foreach (var data in fieldDatas)
                        reqStringBuilder.Append("&").Append(data.Key).Append("=").Append(data.Value);
                }                    
				req = new Request(method.ToString(), reqStringBuilder.ToString());
				break;			
					
			case HTTPMethod.NONE:
				break;		
		}		
		
		if(req != null)
			AddHeaderDatas(req);
		
		return req;
	}

    // handle Q
    public void EnqueueReqQ(RequestData reqData)
    {
        reqQ.Enqueue(reqData);
    }

    public RequestData DequeueReqQ()
    {
        if (IsEmptyReqQ() == true)
            return new RequestData();

        return reqQ.Dequeue();
    }

    protected bool IsEmptyReqQ()
    {
        return reqQ.Count == 0;
    }

    private void Update()
    {
        // 매 프레임 이벤트Q가 비어있는지 현재 통신중인 요청이 있는지 체크합니다.
        if (IsEmptyReqQ() == true || isTransmitData == true)
            return;

        // 이벤트Q로부터 대기중인 요청을 받아옵니다. 
        RequestData reqData = DequeueReqQ();

        // 해당 요청 데이터를 통신 프로세스로 넘깁니다.
        HttpsCall(reqData);
    }

    /// <summary>
    /// Http 통신을 시작합니다.
    /// </summary>
    /// <param name="reqData">프로토콜 요청 데이터가 모인 구조체입니다.</param>
    private void HttpsCall(RequestData reqData)
	{
        if (isTransmitData == true)
			return;

		// 통신전 초기화를 위해 기존 데이터들을 지워줍니다.
		ClearData();
		
		// 통신중을 알리는 플래그를 체크합니다.
		// ClearData()를 통해 해당 플래그가 false가 되기 때문에 반드시 이후에 체크를 해야합니다.
		isTransmitData = true;

		method 			 = reqData.GetHttpMethod();
		reqStr 			 = reqData.GetReqStr();
        type             = reqData.GetRequestDataType();
        fieldDatas 		 = reqData.GetFieldDatas();
		headerDatas 	 = reqData.GetHeaderDatas();


        // 코루틴을 통해 통신 요청
        StartCoroutine("HttpsRequest");
	}

	private IEnumerator HttpsRequest()
	{
		// 통신 방식에 근거해 알맞은 리퀘스트를 생성합니다.
		Request req = CreateCurrentReq();
        Debug.Log("<color=green>@@@ Transmitter [Request] uri : " + req.uri+"</color>");
		float waitTime = 0f;
		int tryCount = 0;
		
		if (req == null)
		{
			Debug.Log("req is null. need handle error");
			yield break;
		}		

		// 통신을 시작합니다.
		// 인풋을 방지합니다.
		req.Send();
		
		//if(needBlock== true)
		//	Utility.PushInputBlockRoot();
		
        // 요청이 처리되기 전까지 대기합니다.
        while (!req.isDone)
        {
            // errorTime 초 이상으로응답을 못받으면 error 
            if (waitTime >= SalinConstants.errorTime)
            {
	            if (tryCount < SalinConstants.retryCount)
	            {
		            ++tryCount;
                    //Utility.OpenToast(TableManager.Instance.GetLanguage("40013", tryCount.ToString()));
                    req = CreateCurrentReq();
		            req.Send();
		            waitTime = 0;
	            }
	            else
	            {
                    

                    Debug.Log(string.Format("{0:F2}", waitTime) + "sec delay occur!! Check your network plz    exception : " + req.exception);	             
		            //"protocol time out."
		            //Utility.OpenPopup(TableManager.Instance.GetLanguage("10108"), TableManager.Instance.GetLanguage("30005"), Application.Quit , false);
		            yield break;   
	            }	            
            }         

            waitTime += Time.deltaTime;
            yield return null;
        }

        try
        {
            // 통신 완료 후 데이터를 처리합니다.
            // 다시 인풋을 받습니다.
            //if(needBlock== true)
            //	Utility.RemoveInputBlockRoot();
            ReceiveHttps(req);
        }
        catch (Exception e)
        {
            Debug.LogError($"Server Error : {e}");
            throw;
        }
        finally
        {
            //AppManager.Instance.SetInputBlock(false);
        }

	}

	/// <summary>
	/// 통신 완료후 데이터를 분석하여 처리합니다.
	/// </summary> 
	/// <param name="req">통신이 완료된 리퀘스트 객체를 받습니다.</param>
	private void ReceiveHttps(Request req)
	{
		ResponseData data = null;
		bool isSuccess = false;
		int errorCode = (int) ErrorCode.Error;
		
		try
		{   

			Debug.Log("<color=green>@@@ Transmitter [Receive] uri : " + req.uri +"</color>   response : " + req.response.Text);

			// 통신 완료후 에러가 없다면 리퀘스트 객체의 json을 통해 ResponseData 를 얻어옵니다.
			// 이 데이터는 protocolCallback 를 위한 데이터입니다.
			if (req.exception == null)
				data = JsonToResponseData(req.response.Text);

			// data 에 code를 체크해 통신의 성공여부를 확인합니다. 
			// 이 데이터는 successCallback 를 위한 데이터입니다.
			if (data != null)
			{
				isSuccess = data.errorCode.Equals((int)ErrorCode.Success);
				errorCode = data.errorCode;
			}			
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
		finally
		{
            if(isSuccess)
            {
                
                SalinCallbacks.OnEvent(data, type);
                
            }
            else
            {
                 SalinCallbacks.OnError((ErrorCode)data.errorCode,type);
            }

            // 각 콜백, 데이터 유무를 확인하여 콜백 실행
            ////아직 어떤 식으로 처리 해야할지 정해야됨
            //errorHandler.ExcuteCallback(errorCode, () =>
            //{
            //    callback.OnEvent(isSuccess, data, type);

            //    //if (protocolCallback != null)
            //    //	protocolCallback(data);

            //    //if (successCallback != null)
            //    //	successCallback(isSuccess);
            //});


            // 통신완료후 데이터를 초기화합니다.
            // 해당 함수 안에서 통신중을 알리는 플래그가 해제됩니다. - isTransmitData
            ClearData(); 
		}
	}

	/// <summary>
	/// 필드 데이터를 해쉬테이블로 만들어 리턴합니다.
	/// </summary>
	/// <returns>필드데이터가 들어간 해시테이블입니다.</returns>
	private Hashtable GetFieldTable()
	{
		Hashtable table = new Hashtable();				
		var e = fieldDatas.GetEnumerator();
		while (e.MoveNext())		
			table.Add(e.Current.Key, e.Current.Value);	
						
		return table;
	}


	private void AddHeaderDatas(Request req)
	{
		if(headerDatas == null || headerDatas.Count == 0)
			return;
		
		var e = headerDatas.GetEnumerator();
		while (e.MoveNext())		
			req.SetHeader(e.Current.Key, e.Current.Value);
	}

	/// <summary>
	/// json 문자열을 Dictionary<string, object> 형태로 변환합니다.
	/// 이 때 가독성을 위해 해당 딕셔너리를 상속받은 ResponseData 를 사용합니다.
	/// 추가로 json 데이터와 에러코드 데이터를 가지고 있습니다.
	/// </summary>
	/// <param name="jsonStr"> ResponseData로 변환할 문자열입니다. json 형식을 기반으로 컨버팅합니다. </param>
	/// <returns>컨버팅된 ResponseData입니다.</returns>
	public ResponseData JsonToResponseData(string jsonStr)
	{		
		ResponseData responseData = new ResponseData();
				
		// 제이슨 데이터 할당
		JsonData jsonData = JsonMapper.ToObject(jsonStr);		
		responseData.SetJsonString(jsonStr);
		responseData.SetJsonData(jsonData);		
		
		// 순회를 위해 IEnumerator 를 가져온 뒤 데이터를 Dictionary에 추가합니다.
		var e = jsonData.Keys.GetEnumerator();
		while (e.MoveNext())
			responseData.Add(e.Current, jsonData[e.Current]);

		// 에러코드 할당
		if (responseData.ContainsKey(SalinAPIKey.code))
		{
            int errorCode = SalinConstants.defaultInt; 
            int.TryParse((responseData[SalinAPIKey.code].ToString()),out errorCode);
			responseData.SetErrorCode(errorCode);
		}
		else if (responseData.ContainsKey("status"))
		{
			JsonData statusData = null;
			statusData = (JsonData) responseData["status"];
			if (statusData["message"] != null)
			{
				int errorCode = statusData["message"].ToString() == "SUCCESS" ? 200 : 0;
				responseData.SetErrorCode(errorCode);
			}			
			else
				Debug.LogError("There is no code in the json. must something is wrong!!!!");
		}
		else
			Debug.LogError("There is no code in the json. must something is wrong!!!!");

		return responseData;
	}


}