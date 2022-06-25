using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.ConsoleTests
{
    public class BitcoinSecrets
    {
        public const string MainKey = "cUd8hYvhpfw2jqETpcCFh1iVDHWBQzpowTjsEVy56b6JEe4z5x7q";
        public static List<string> Keys = new List<string>
        {
            "cU72YKBsMEW1sfBhzupkjF69mKeGpHg9FgrKjb9v1EpG32SaUL6D",
            "cSTwbeRApbGUeiyoR1aC1BDrdGVJFUu4LDF5Ms166J2zPiXzheGU",
            "cSrPaxwnfte2p2KycGMW9VkXBGS6GCSHR21GR6Rw8AnteSmTGFuy",
            "cMnbh8T6jw4PoiTu57zNqsfngs8SX3DshVJtvvL1bxYWfJ5G6Anu",
            "cN2SD7hCgXYh7Kmx1pxKZ5FNa4t8hfm1kH5QVk8HLujtnoiPaD8G"
        };

        public static BitcoinSecret Main = new BitcoinSecret(MainKey, Network.TestNet);

        public static BitcoinSecret[] Secrets = Keys.Select(k => new BitcoinSecret(k, Network.TestNet)).ToArray();
    }
}
