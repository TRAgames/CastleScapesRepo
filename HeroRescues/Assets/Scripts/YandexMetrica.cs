#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Collections.Generic;

    public static class YandexMetrica
    {
        public static void Send(string eventName)
        {
#if UNITY_EDITOR
            SendEditor(eventName, string.Empty);
#else
            YandexMetricaSend(eventName, string.Empty);
#endif
        }

        public static void Send(string eventName, IDictionary<string, string> eventParams)
        {
            if (eventParams == null || eventParams.Count == 0)
            {
                Send(eventName);
                return;
            }

            var eventParamsJson = JsonUtils.ToJson(eventParams);

            if (string.IsNullOrEmpty(eventParamsJson))
            {
                Send(eventName);
                return;
            }

#if UNITY_EDITOR
            SendEditor(eventName, eventParamsJson);
#else
            YandexMetricaSend(eventName, eventParamsJson);
#endif
        }

#if UNITY_EDITOR
        private static void SendEditor(string eventName, string eventParams)
        {
                if (string.IsNullOrEmpty(eventParams))
                {
                    Debug.Log($"<color=green>YandexMetrica</color>: {eventName}");
                    return;
                }

                Debug.Log($"<color=green>YandexMetrica</color>: {eventName}; {eventParams}");
        }
#endif

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool YandexMetricaSendInternal(string eventName, string eventData);

        private static void YandexMetricaSend(string eventName, string eventData)
        {
                YandexMetricaSendInternal(eventName, eventData);
        }
    }
