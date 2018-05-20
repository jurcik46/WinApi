using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.DataModels.Data;
using Newtonsoft;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WinApi.DataModels
{
    class Options
    {
        public Options()
        {
            OptionsData test = new OptionsData {
                ApiLink = "111111111111",
                ChannelName = "channel",
                ProcessName = "processs",
                ProgramPath = "programpatcasda"
            };

            SaveOption(test);
            LoadOption();
        }

        public void SaveOption(OptionsData data)
        {
            File.WriteAllText("options.json", JsonConvert.SerializeObject(data));
        }

        public void LoadOption() {

            OptionsData movie1 = JsonConvert.DeserializeObject<OptionsData>(File.ReadAllText("options.json"));
            string json = JsonConvert.SerializeObject(movie1,Formatting.Indented);
            Console.Write(json);
        }

    }
}
