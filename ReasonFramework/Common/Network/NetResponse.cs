using System;
using System.Collections.Generic;
using System.Linq;

namespace ReasonFramework.Common
{
    public class NetResponse
    {
        private Dictionary<string, string> _params;
        public PacketTypes GetMethod
        {
            get
            {
                if (!IsContainsKey("method"))
                    return PacketTypes.nil;
                else
                {
                    var pt = PacketTypes.nil;
                    if (Enum.TryParse<PacketTypes>(GetValue("method"), out pt))
                        return pt;
                    else
                        return PacketTypes.nil;
                }
            }
        }
        private string _error;
        public string GetError
        {
            get
            {
                return _error;
            }
        }

        public bool IsError
        {
            get { return !string.IsNullOrEmpty(_error); }
        }

        public NetResponse(string data)
        {
            _params = new Dictionary<string, string>();

            var mas = data.Split(new char[] { '&' });
            foreach (var s in mas)
            {
                var temp = s.Split(new char[] { '=' });
                _params[temp[0].ToLower()] = temp[1];
                
            }

            if (!IsContainsKey("error") || _params["error"].Length == 0)
                _error = string.Empty;
            else
                _error = _params["error"];
        }

        public bool IsContainsKey(string key)
        {
            return _params.ContainsKey(key.ToLower());
        }

        public string GetValue(string key)
        {
            return _params[key.ToLower()];
        }
    }
}
