using LitJson;
using System.Collections;
using System.Collections.Generic;

namespace SalinSDK.ExtensionMethod
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// jsondata 에 키값이 있는지 유무를 판단합니다.
        /// </summary>
        /// <param name="data">확장 메서드 선언 파라미터입니다. - JsonData</param>
        /// <param name="key">검사 할 키값입니다.</param>
        /// <returns></returns>
        public static bool JsonDataContainsKey(this JsonData data, string key)
        {
            // 기본 반환값은 false 입니다. 아래 식을 통과 못하면 그대로 반환합니다.
            bool result = false;

            // 데이터의 유무를 파악한 뒤
            if (data != null)
            {
                // IDictionary 로 저장합니다.
                IDictionary tdictionary = data;
                // 키값이 있으면 트루를 반환합니다.
                if (tdictionary.Contains(key))
                    result = true;
            }

            return result;
        }
    }
}
