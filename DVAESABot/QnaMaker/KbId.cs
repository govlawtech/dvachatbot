using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVAESABot.QnaMaker
{
    public class KbId
    {
        // Contains mapping of Factsheet Code (e.g. MRC04) to knowledge base ID
        public static Dictionary<string, string> kbIDs;

        static KbId()
        {
            kbIDs = new Dictionary<string, string>();
            kbIDs.Add("MRC01", "8f655e4e-b891-4d48-abe4-94f9109aa4e6");
            kbIDs.Add("MRC04", "326f5639-1392-4929-93b6-1dd1302a7ebe");
            kbIDs.Add("MRC05", "10278d30-5f6a-4aa4-abfc-506524b52276");
            kbIDs.Add("MRC07", "15eab354-34a4-48f8-9095-9f830902ee4d");
            kbIDs.Add("MRC08", "2ed914e9-b684-4792-b9f7-99804f07feba");
            kbIDs.Add("MRC09", "cc6f33e2-142a-40cb-b63e-bcd54beafd6e");
            kbIDs.Add("MRC10", "eaa095f2-5cf3-44fe-8c01-8eb45aa74235");
            kbIDs.Add("MRC17", "b43ad95f-2d69-4a17-81eb-175d1608e3eb");
            kbIDs.Add("MRC18", "2145d130-c245-4ce8-8874-9a320b3add5c");
            kbIDs.Add("MRC25", "1d9f5208-b773-4472-9d9b-bbd42d30216a");
            kbIDs.Add("MRC27", "4d29b793-113f-4945-9e11-e139a2df3273");
            kbIDs.Add("MRC30", "e104e7b6-1d06-4522-b833-c724c79bc9f2");
            kbIDs.Add("MRC31", "0e462241-4c0d-423c-b350-8e501af58321");
            kbIDs.Add("MRC33", "386e6d60-95aa-4e01-b730-b6f9e59fe0cb");
            kbIDs.Add("MRC34", "28ca50ed-3e10-4b58-886e-d4c449119b89");
            kbIDs.Add("MRC35", "422b5c53-03ae-4270-bb9f-cc702037f592");
            kbIDs.Add("MRC36", "76c9ae7c-b2ab-4ffa-a87b-640c74d1a8a5");
            kbIDs.Add("MRC39", "ed165336-edc0-4cef-8d62-9462c6a72b03");
            kbIDs.Add("MRC40", "a9d0ef6f-836b-43e6-ad93-87ed6cf882eb");
            kbIDs.Add("MRC41", "90b94b57-ddbc-4702-be1b-14d5109f87f2");
            kbIDs.Add("MRC42", "95f354f8-93ec-44a2-aa8c-73b1ba185efb");
            kbIDs.Add("MRC43", "c7578a86-abfd-4503-b7d2-871c264091a1");
            kbIDs.Add("MRC45", "e62e5a91-c5b0-4853-a443-74c98af19a43");
            kbIDs.Add("MRC47", "fa082252-5687-45b9-80cc-1c6d77cb2e7f");
            kbIDs.Add("MRC49", "b28c2d87-f64c-4faa-bfe0-91711d2deff4");
            kbIDs.Add("MRC50", "06c15cc3-349e-48ed-9290-224c51731d35");
        }
    }
}