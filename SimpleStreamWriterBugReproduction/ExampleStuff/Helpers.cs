
namespace SimpleStreamWriterBugReproduction
{
    

    public class Helpers
    {


        public static async System.Threading.Tasks.Task ToJSON(System.IO.Stream s, object o)
        {
            // https://stackoverflow.com/questions/27197317/json-net-is-ignoring-properties-in-types-derived-from-system-exception-why
            // JsonSerializer.SerializeAndIgnoreSerializableInterface(this, s);

#if true
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(s))
            {
                using (Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer))
                {
                    Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
                    ser.Formatting = Newtonsoft.Json.Formatting.Indented;
                    ser.Serialize(jsonWriter, o);
                    jsonWriter.Flush();
                }
            }

            await System.Threading.Tasks.Task.CompletedTask;
#else
            System.Text.Json.JsonSerializerOptions jso = new System.Text.Json.JsonSerializerOptions();
            jso.IncludeFields = true;
            jso.WriteIndented = true;
            // jso.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            // System.Text.Json.JsonSerializer.Serialize(s,this, this.GetType(), jso);
            await System.Text.Json.JsonSerializer.SerializeAsync(s, o, o.GetType(), jso);
#endif
        } // End Constructor 

    }
}
