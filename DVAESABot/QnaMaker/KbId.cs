﻿using System;
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
            kbIDs.Add("BR04", "726b643c-b36c-442b-8bae-b9f0c5f8dd9a");
            kbIDs.Add("CEP01", "151d36cf-2fc4-45d0-b164-233798b543ec");
            kbIDs.Add("CEP03", "ff1e1b8a-5efd-4137-b783-511c610b4f98");
            kbIDs.Add("CON01", "cca0a2b9-e93c-49e4-b817-d9256c76fd17");
            kbIDs.Add("CON02", "36e47c4a-baa0-4443-9973-fdd6cbbb1c4f");
            kbIDs.Add("CON03", "3682cb63-7086-4ea8-823c-39587101c3bb");
            kbIDs.Add("CON04", "3de5350a-27a0-46a6-8806-e39a0ddbb134");
            kbIDs.Add("CON05", "a6beac6f-932b-451e-9c5b-b364a02f6d38");
            kbIDs.Add("CON06", "875efcb1-e038-4660-aced-860228db6a89");
            kbIDs.Add("CON07", "255cf0dc-4eaf-4c50-90aa-78a03c383df5");
            kbIDs.Add("CON08", "97696957-3a6a-4ac4-a566-390cf922f03b");
            kbIDs.Add("DP01", "a67e2530-75b3-4032-8921-f8c2545f566c");
            kbIDs.Add("DP02", "3ffe8c0d-10bd-45ab-875e-55a56a8452e8");
            kbIDs.Add("DP13", "dff3a796-a4ab-491d-9ce6-0576202bbe66");
            kbIDs.Add("DP15", "b16f3c8c-3f06-42a5-b7aa-ef35a1305666");
            kbIDs.Add("DP18", "52d5b03a-79c0-41b8-bdcf-6b80013e3def");
            kbIDs.Add("DP22", "e561a30f-4769-4bfe-a1f1-6f5c20ac5d2d");
            kbIDs.Add("DP23", "0b8d978b-bafb-4cb9-83fd-e7192a4d960f");
            kbIDs.Add("DP28", "e55252ca-4dc9-4cea-a280-00754ca10c5d");
            kbIDs.Add("DP29", "18f7ae46-a449-4d7f-9e31-5e07277ed97a");
            kbIDs.Add("DP30", "8ad1800d-3267-4a49-840b-4298cbf82cb8");
            kbIDs.Add("DP42", "c634b9ef-0197-480c-9cde-e0e2d072af80");
            kbIDs.Add("DP43", "6a095532-6c93-4619-8656-199500b3ecc8");
            kbIDs.Add("DP50", "65e02d27-0bc9-46f0-b8ec-3a8a19bdf68f");
            kbIDs.Add("DP60", "936cb3af-715e-438b-a10c-824ca1551e1e");
            kbIDs.Add("DP68", "8e9f1524-a656-4011-868b-f8edee724731");
            kbIDs.Add("DP71", "8728aad0-5759-4e9e-8955-173bd88593db");
            kbIDs.Add("DP72", "b007bcdc-4b68-494e-b52c-574432174038");
            kbIDs.Add("DP73", "947c1dfc-dcf4-4e3c-b7e4-dc08da041273");
            kbIDs.Add("DP74", "1b23d379-1c18-4b35-acf3-f32611fcd2ff");
            kbIDs.Add("DP75", "da545a4f-15c4-41f3-a3c8-c64fb517c4c1");
            kbIDs.Add("DP76", "0911e170-370e-4ada-a81c-ab8be29ee878");
            kbIDs.Add("DP78", "5be1cf81-dd4a-42fb-92d5-66b23a74d08c");
            kbIDs.Add("DP79", "c9dc1024-2252-4205-8519-f9fe25ba5ea6");
            kbIDs.Add("DP81", "e5ef9759-d344-43ef-b2dc-11b3c76f0296");
            kbIDs.Add("DP82", "0b8a3fa2-ca69-4586-9eb0-b80ca8fb9c2e");
            kbIDs.Add("DP83", "f787c933-b3ee-49e1-b8d7-be1c9236a698");
            kbIDs.Add("DP84", "15fca3c4-44e4-40f7-bd35-36391faaa862");
            kbIDs.Add("DP85", "cc4b6592-660c-4540-9c6a-a5a61185e9af");
            kbIDs.Add("DVA03", "c766ede0-31ed-40c5-81c9-be7b8031bb5f");
            kbIDs.Add("DVA06", "ea5f9f77-7a90-417f-a0dc-f7013f4acd6f");
            kbIDs.Add("DVA19", "589ceb5b-b643-4ee1-8cb8-44b882a3b1b3");
            kbIDs.Add("DVA21", "5fe00750-b423-4219-9868-00bd61d4e4da");
            kbIDs.Add("DVA23", "9c532e59-8dfb-436e-bb33-d59c7126aa9b");
            kbIDs.Add("F111", "5bd0e8f7-62a1-46b1-9bc7-029d78299e8e");
            kbIDs.Add("FIP01", "4486479b-f185-4a42-94a1-7c9c91136da8");
            kbIDs.Add("FIP02", "d70087c5-1e15-448d-8629-9012b237e6bb");
            kbIDs.Add("FIP04", "46a90278-1614-4bdb-9650-c77d47005223");
            kbIDs.Add("GS01", "95fbff22-65e4-4b99-9757-ff8c3d9006f8");
            kbIDs.Add("GS02", "8f570135-5dc7-4870-a277-0aa668915de0");
            kbIDs.Add("GS03", "18b3102f-2f37-4705-b2f9-a33b64f131cf");
            kbIDs.Add("GS04", "fd11f947-bd4b-4ef1-be3a-2b037a30f5cb");
            kbIDs.Add("GS05", "5fdf6670-8381-4519-8a6d-cadae0e93626");
            kbIDs.Add("HAC01", "399cb87d-e2c5-4e28-b920-c8922d5dfd75");
            kbIDs.Add("HAC02", "3223c0cc-7606-4383-8dae-8858e0c15d17");
            kbIDs.Add("HCS01", "aecefa92-7315-4316-bf70-a700de64e47a");
            kbIDs.Add("HCS05", "4f8b0c6d-4da0-4d43-99af-d08bb6c9b2d2");
            kbIDs.Add("HCS10", "2bb354a0-da46-4842-839b-c225d3eb411f");
            kbIDs.Add("HIP01", "30512f8e-194a-447d-b7e6-d5753a7345ce");
            kbIDs.Add("HIP06", "ddfb34f2-8af6-46e2-8b23-2278a05a957a");
            kbIDs.Add("HIP40", "1f7a437c-954e-440c-a1f2-366ac7fc4493");
            kbIDs.Add("HIP72", "73581886-70ae-4295-836e-6eb316181a9c");
            kbIDs.Add("HIP80", "e114216a-605f-47a8-a7cd-612b8c40b803");
            kbIDs.Add("HIP90", "027ad8fa-50d9-4f5a-a5cf-904af789dcdc");
            kbIDs.Add("HSV01", "59b52714-2b4f-473f-a2cd-b9ea1ec40acf");
            kbIDs.Add("HSV02", "3c55e8e9-11bb-49d7-bab5-835bc35fc171");
            kbIDs.Add("HSV03", "077bcbc2-7132-4a93-82eb-30569bbc8dd1");
            kbIDs.Add("HSV05", "be3b50e3-9383-4c28-9f4a-9bacff0bcbdc");
            kbIDs.Add("HSV06", "7af84a87-bc61-40d8-8f39-5e6d4e1cdbd2");
            kbIDs.Add("HSV10", "aad4bb59-5294-43f4-8597-65ea3f20de9b");
            kbIDs.Add("HSV100", "d47f563a-9d98-431d-8de5-eaec3f22017a");
            kbIDs.Add("HSV101", "da7b901a-56d7-4824-8d4d-843f7242d145");
            kbIDs.Add("HSV107", "e96746b7-0045-4f50-a076-f885e1e157ce");
            kbIDs.Add("HSV108", "0c5f0994-23d5-4b55-880c-235ef335a24a");
            kbIDs.Add("HSV109", "77d13aa3-bcaa-472d-acb0-5b430fb5da2f");
            kbIDs.Add("HSV120", "b695237f-c975-4b05-8381-7834aa33fec8");
            kbIDs.Add("HSV13", "a1b6cca5-1a0c-4ff5-b615-01cad9fb1f33");
            kbIDs.Add("HSV131", "48240c45-ff01-4787-81f5-8da3ae004bd8");
            kbIDs.Add("HSV132", "8da2bae9-4e70-4170-ba31-784a800c50c0");
            kbIDs.Add("HSV136", "9aefe4b0-8ddb-4050-8b9b-2b93c61113ea");
            kbIDs.Add("HSV137", "19263713-fb0c-48a1-bc7f-764f914a9dc1");
            kbIDs.Add("HSV139", "f397a197-a3ba-4098-a9a0-b439b20bf95e");
            kbIDs.Add("HSV14", "3851acb0-39ae-498c-bbd6-267cfcb67c70");
            kbIDs.Add("HSV140", "9791a48f-f537-4dec-b1fc-033733d8aae9");
            kbIDs.Add("HSV16", "858ef2d4-aea8-4001-9cc3-f5b563fcdd4c");
            kbIDs.Add("HSV17", "cf732c25-6ac9-4f94-afdb-4cb132c93c14");
            kbIDs.Add("HSV18", "dbfd0ce1-69c8-46a2-bfb7-da257ff7b645");
            kbIDs.Add("HSV19", "9e32bdc4-aab6-4cae-accd-74281c19ece6");
            kbIDs.Add("HSV20", "aadf6507-b8ac-4970-9e8d-d21c139285d6");
            kbIDs.Add("HSV21", "c5f02a80-fb92-461e-be9d-1a3cfdc2bb4f");
            kbIDs.Add("HSV22", "9f8d8d8a-af65-4316-b08d-6fb29dbf390f");
            kbIDs.Add("HSV23", "a9cfebc6-19b6-4a74-ac25-d0f98ba12ddc");
            kbIDs.Add("HSV27", "0c8e0600-fd24-44ce-aae9-af6c4f0b41d1");
            kbIDs.Add("HSV29", "60718cd1-f6b4-457b-889a-ff9238fafdfc");
            kbIDs.Add("HSV30", "1e7c7713-b853-46da-bff5-7809a9e28880");
            kbIDs.Add("HSV59", "f88ccda2-20a6-47b0-85e1-f4118587df74");
            kbIDs.Add("HSV60", "b70b120d-edd7-4a68-b984-68af14238ac5");
            kbIDs.Add("HSV61", "4bd8f53f-9e50-4097-a746-e73fb992eaab");
            kbIDs.Add("HSV62", "e54eb3f8-b6b8-4f81-9a55-640b9d28474e");
            kbIDs.Add("HSV64", "52ed0b06-28f0-4fc6-aef8-9a1be9b0278d");
            kbIDs.Add("HSV65", "536db852-ca0c-4fa1-ae63-25059e3a3e9d");
            kbIDs.Add("HSV69", "34fa387b-4046-4b69-94c5-497150f9b9e0");
            kbIDs.Add("HSV74", "6527a4c8-28be-42e2-9c1a-68aba55f5939");
            kbIDs.Add("HSV77", "78ab84d5-d188-4786-a950-937a85c7a796");
            kbIDs.Add("HSV80", "18812da4-6c0d-4a6e-a4ad-c8c8de8db150");
            kbIDs.Add("HSV92", "ef36aafd-0ffe-479b-b8f0-7212eff42713");
            kbIDs.Add("HSV93", "51269431-f478-4741-992b-ce078afdee9a");
            kbIDs.Add("HSV97", "5af8f23f-46f6-4a7f-b6ae-0a44e0b6430a");
            kbIDs.Add("HSV99", "7813a930-fd92-45e8-9178-ffa41d7f9976");
            kbIDs.Add("IP01", "283324cc-8589-4c6f-b31c-e6d28a34da59");
            kbIDs.Add("IS01", "247939fe-7555-4339-8b83-7f3c4d19b8fa");
            kbIDs.Add("IS02", "5514e210-7dcd-4a1d-8b1e-339b5986d976");
            kbIDs.Add("IS03", "5db66176-372f-4b97-84de-9bc7b1f9f309");
            kbIDs.Add("IS04", "5ab5a582-4eac-4b81-99ce-055203ae7096");
            kbIDs.Add("IS05", "c7a839af-5ec1-414a-9b54-93bd949f7c24");
            kbIDs.Add("IS06", "c26ac702-d267-4495-b79c-5002dd00c9a2");
            kbIDs.Add("IS07", "fcc23307-0efd-4178-b664-c909bce9081b");
            kbIDs.Add("IS08", "91f244a9-0724-4b8e-87bd-f7043ab05da9");
            kbIDs.Add("IS09", "643e916a-1980-4168-9c27-4e44b45c39b7");
            kbIDs.Add("IS10", "88b409a0-98eb-4b49-a37f-9ef1ca5c8411");
            kbIDs.Add("IS101", "cda1d3bb-9e30-4510-9c19-5400171e7977");
            kbIDs.Add("IS103", "346ae28d-4998-4ed8-aaa8-c9165fa7f759");
            kbIDs.Add("IS104", "41fd2f55-2e50-4228-ad19-c8ac899f97a5");
            kbIDs.Add("IS105", "88191596-8418-48cd-a498-5f22cb1d27f3");
            kbIDs.Add("IS106", "1dad5259-ec6f-4abf-8d14-82b98b61b7b2");
            kbIDs.Add("IS115", "493354a0-3ce1-457e-befa-39e6c5868189");
            kbIDs.Add("IS116", "07dff23a-f385-44f9-8423-cdfaa112700d");
            kbIDs.Add("IS117", "a3fbb81b-53f1-4b2b-bbe9-0bf2c10c5d76");
            kbIDs.Add("IS12", "b7015c4e-98e0-43e2-9646-daa2741bbff4");
            kbIDs.Add("IS121", "3437d77e-9475-4477-9422-292cd015be34");
            kbIDs.Add("IS122", "0b2161dd-3c51-432a-b3a9-98eccf8e3db7");
            kbIDs.Add("IS125", "b317c07c-e136-4e4f-94ec-af310ac80222");
            kbIDs.Add("IS126", "c64e091e-621d-4630-bea2-71b5aa139b5e");
            kbIDs.Add("IS135", "460c1fd6-a435-4cae-a993-c58c6a3f396a");
            kbIDs.Add("IS137", "87cd995e-0cf1-46d1-905c-9ed7d3eaf7cc");
            kbIDs.Add("IS138", "c57bb922-2ff5-4b00-a4c9-18efab70c3f1");
            kbIDs.Add("IS139", "e177d316-cd91-43c0-869e-3dbb5caf9089");
            kbIDs.Add("IS140", "bbb3965b-20f6-4c24-acf0-e1575f278b00");
            kbIDs.Add("IS141", "151096a1-9083-4678-b6d4-3e7962bafd77");
            kbIDs.Add("IS142", "b10591e2-bf0c-4e15-b5c6-38a7038fa530");
            kbIDs.Add("IS143", "75891b99-415f-4e2a-8ea2-a845c186b2c9");
            kbIDs.Add("IS144", "b5f899d7-5aa8-404d-b117-e857f4df8c61");
            kbIDs.Add("IS145", "0d7b4643-977b-4569-a95f-2364471835f4");
            kbIDs.Add("IS147", "753274a3-7c3e-4abb-b0d3-472e30b79853");
            kbIDs.Add("IS15", "92d3ab26-5ade-4291-8dc0-0298d6d67ba3");
            kbIDs.Add("IS150", "5e7979f5-f48d-4b85-b573-5fa58cd0387f");
            kbIDs.Add("IS151", "8c7a506c-8182-4c19-a943-cf78aa3f2aac");
            kbIDs.Add("IS154", "a32c9efc-e2c0-4564-bd85-054fb6d8ef41");
            kbIDs.Add("IS155", "5ad3b392-0e33-48ac-a303-189725d4edd3");
            kbIDs.Add("IS156", "48b46686-3f23-458c-8571-1b32fa464fa2");
            kbIDs.Add("IS158", "3312c24f-5579-4e16-8be9-ec90fead6ee5");
            kbIDs.Add("IS159", "0861a877-6927-4ba8-b4be-aed60fc6d578");
            kbIDs.Add("IS16", "acdd0069-75ab-4cad-8ddf-7be1cafb3a91");
            kbIDs.Add("IS160", "b2949618-a557-4aef-a599-f4827c1ff4f1");
            kbIDs.Add("IS161", "c071ef2a-9e75-4452-90b0-a7bbb7e85e82");
            kbIDs.Add("IS163", "5dc865e4-b5e8-44ea-989d-39fb9821612c");
            kbIDs.Add("IS164", "6ecf6baf-d386-4bf0-9e4f-e0dcb722754c");
            kbIDs.Add("IS165", "e5d4b6d0-0823-4893-8517-d5935058237b");
            kbIDs.Add("IS166", "05806cd7-6e28-40de-b2ad-737f22903f22");
            kbIDs.Add("IS167", "b74c8f57-a3d7-457c-b719-3799da0777c2");
            kbIDs.Add("IS168", "76c330d2-1182-47a0-aea5-1cce2da9088a");
            kbIDs.Add("IS18", "9a584566-d432-4073-840d-0d3bf464058f");
            kbIDs.Add("IS184", "50f0858c-0d46-4f05-adee-5ca1ce8c41eb");
            kbIDs.Add("IS185", "4977b41d-9722-4aee-86c2-a45b8fdf21f5");
            kbIDs.Add("IS186", "143b88aa-2cfa-4672-a862-d481afad7c5a");
            kbIDs.Add("IS187", "a3c38e87-dcd0-49f7-83ca-0463b83f72b2");
            kbIDs.Add("IS188", "d69d1ef4-f29b-4aa2-90fc-59e11d15871c");
            kbIDs.Add("IS19", "0a7f5115-969c-4931-81df-1750c7444e53");
            kbIDs.Add("IS29", "c751c245-f7da-4a33-8a4e-2fade3d2a91a");
            kbIDs.Add("IS30", "4fbc5d94-9875-469d-b971-8a11a5e44cad");
            kbIDs.Add("IS34", "795f9a6f-d248-4aab-8043-e39b85433b52");
            kbIDs.Add("IS35", "bff2e643-e5b9-453c-abd1-73268f6e6ffb");
            kbIDs.Add("IS44", "d7775158-8003-44bd-bca6-d468ffab855c");
            kbIDs.Add("IS45", "e2215d11-e7f8-4f2a-9b48-c159f5df8fa9");
            kbIDs.Add("IS46", "711b17dd-ab7b-4790-9378-c74062773f8c");
            kbIDs.Add("IS47", "290fd439-484f-4f8b-8fad-2ac14cf67bd3");
            kbIDs.Add("IS48", "c6a1daef-b349-4424-98c2-a18194be7305");
            kbIDs.Add("IS50", "efd6f8c7-ebd5-49e1-bd95-71fa8a0ed99e");
            kbIDs.Add("IS57", "2032c728-0bdf-405d-b714-71482226bc8a");
            kbIDs.Add("IS58", "4d36e958-984e-4ec2-b0c8-6cf9312b23bc");
            kbIDs.Add("IS65", "627ecc16-2c2d-4f63-9650-04af502fde3a");
            kbIDs.Add("IS71", "b7f5a2e2-a67a-4909-93c7-ac674da97633");
            kbIDs.Add("IS72", "bc19a5ed-3e16-475f-9c5e-2f80657dd34e");
            kbIDs.Add("IS73", "2ba3fc9d-4b71-48a8-92bd-661682c6a521");
            kbIDs.Add("IS74", "fa295203-32be-40fe-952b-8e11bbd12512");
            kbIDs.Add("IS75", "e9a1840e-b3ba-4566-b062-737bbec22ea0");
            kbIDs.Add("IS77", "0a49b8d3-b209-45ed-9e63-8ac94eba2b5f");
            kbIDs.Add("IS79", "defaf1df-62e8-4d0a-a06c-2cfc5a7c35b7");
            kbIDs.Add("IS81", "5967716e-8813-48bc-b449-c78f26415da3");
            kbIDs.Add("IS82", "c80087b2-18d6-4ed4-a0f5-58a99804308a");
            kbIDs.Add("IS85", "76d229a3-fe33-4ef5-88db-00cad8417c5b");
            kbIDs.Add("IS86", "ca347542-d10a-409e-ba40-6ad6f5f8c5ec");
            kbIDs.Add("IS87", "40f234ff-6b6f-4c78-b658-bcaeef2e62e6");
            kbIDs.Add("IS88", "e6e2b0d3-6911-4b5d-b38f-0aa5075fc11c");
            kbIDs.Add("IS89", "26cf7edf-9efd-47c4-b74d-236c01e54c68");
            kbIDs.Add("IS90", "ba42c184-725b-4996-96d6-75a6dd8adf89");
            kbIDs.Add("IS91", "513f6582-755b-4c20-9956-8d1215043623");
            kbIDs.Add("IS92", "5fd58fed-12aa-4c17-8c84-c16fbeb73c86");
            kbIDs.Add("IS94", "b7609163-dba9-4333-9579-80a8b3aa4f02");
            kbIDs.Add("IS95", "3370ed39-94b5-4791-95ec-4bbeb5ec4daa");
            kbIDs.Add("IS96", "415dfd63-d9b4-4825-87a0-916bd302309b");
            kbIDs.Add("IS97", "1c28c4e0-fdb1-4127-8b9d-e2972c6aa399");
            kbIDs.Add("IS98", "bd40f05f-9263-468b-8930-b09d4248331b");
            kbIDs.Add("IS99", "b39e99f5-f5d8-4fd5-af2d-d24875c91844");
            kbIDs.Add("LEG01", "38f0d655-0c9b-4065-8752-c33816cf064c");
            kbIDs.Add("LEG02", "dd109921-8643-47bf-8849-3f8913afd9e7");
            kbIDs.Add("MCS01", "b0f8cb17-41e0-46b4-9860-7362761e6f9b");
            kbIDs.Add("MCS07", "2a9b03dc-5d1c-4076-8d18-638fa617abf6");
            kbIDs.Add("MCS13", "6721586d-940d-44b0-aa40-c6327c26760e");
            kbIDs.Add("NR01", "80a6c338-0163-4ae7-a5a8-93bc05adfe18");
            kbIDs.Add("POW01", "5cf0b340-6cb0-4afe-9747-0b71b9c24b02");
            kbIDs.Add("POW02", "32b0b27f-4319-45bf-b52d-295127ee04c1");
            kbIDs.Add("VCS01", "435f84e5-80d5-44eb-8e5b-ce32724f938c");
            kbIDs.Add("VCS02", "5eaaa764-1887-4525-beb9-dc16c5e4c1f0");
            kbIDs.Add("VCS03", "11907e59-9faf-427f-8b80-10ba8d099cc3");
            kbIDs.Add("VRB01", "4461afae-35f1-4969-bd6e-d8298079a5f7");
            kbIDs.Add("VRB02", "eb299a29-e468-469c-b0df-f7eabee3e176");
            kbIDs.Add("VRB03", "dfbe1954-8efb-4824-a16c-ccca111b54da");
            kbIDs.Add("VRB04", "9d9b8e80-94a4-424e-88e8-ff8ca21735d4");
            kbIDs.Add("VRB05", "4b90d137-24e4-4b29-8c8b-a7a7b6959894");
            kbIDs.Add("VRB06", "6e567b1b-ef88-4e1f-bd6c-91a91b38c545");
            kbIDs.Add("WG01", "9da3ae40-1e5b-4d8d-8e62-c7cbd830ced3");
            kbIDs.Add("WG02", "b4c92120-c37b-4332-b7bd-6d319b78ea3c");
        }
    }
}