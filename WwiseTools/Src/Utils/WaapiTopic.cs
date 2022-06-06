using System;
using System.Collections;
using System.Collections.Generic;

namespace WwiseTools.Utils
{
    internal class WaapiTopic : IEnumerable<string>
    {
        public static string CoreObjectGet => "ak.wwise.core.object.get";

        private List<string> _topics;

        public WaapiTopic()
        {
            _topics = new List<string>();
        }

        public void AddTopic(string function)
        {
            if (!_topics.Contains(function))
                _topics.Add(function);
        }

        public string Verify(string topic)
        {
            topic = topic.Trim();
            bool result = false;
            string final = null;
            if (_topics.Contains(topic))
            {
                final = topic;
                result = true;
            }
            else
            {
                foreach (var function in this)
                {
                    if (function.ToLower().Contains(topic.ToLower()))
                    {
                        final = function;
                        result = true;
                        break;
                    }
                }
                if (result)
                    WaapiLog.Log($"Warning: No matching topic for {topic}! Using {final} instead!");
            }

            if (!result)
                throw new Exception($"Topic {topic} not available in wwise " +
                                    $"{WwiseUtility.Instance.ConnectionInfo.Version.ToString()}!");
            return final;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _topics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
