using System;
using System.Collections.Generic;
using System.Linq;

namespace ReasonFramework.Common
{
    public class NetRequest
    {
        private Dictionary<string, string> _params;

        public PacketTypes GetMethod
        {
            get
            {
                if (!_params.ContainsKey("method"))
                    return PacketTypes.nil;
                else
                {
                    var pt = PacketTypes.nil;
                    if (Enum.TryParse<PacketTypes>(_params["method"], out pt))
                        return pt;
                    else
                        return PacketTypes.nil;
                }
            }
        }

        public NetRequest(PacketTypes header)
        {
            _params = new Dictionary<string, string>();
            _params["method"] = header.ToString().ToLower();
        }

        public void AddParam(string key, string value)
        {
            _params[key.ToLower()] = value;
        }

        public void AddParam(string key, int value)
        {
            AddParam(key, value.ToString());
        }

        public void AddParam(string key, bool value)
        {
            AddParam(key, value? 1: 0);
        }

        public void RemoveParam(string key)
        {
            if (_params.ContainsKey(key.ToLower()))
                _params.Remove(key.ToLower());
        }

        public string GetParamsString()
        {
            return string.Concat(_params.Select(n => string.Concat(n.Key, "=", n.Value, "&")));
        }
    }
}
