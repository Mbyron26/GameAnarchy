using MbyronModsCommon;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LocalizeGenerator {
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("LocalizeGenerator", "Generate localization", "1.0")]
    [Guid("964A3BA6-AE19-4523-B8BC-C159B07905B7")]
    [ComVisible(true)]
    [ProvideObject(typeof(LocalizeGenerator))]
    [CodeGeneratorRegistration(typeof(LocalizeGenerator), "LocalizeGenerator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    public class LocalizeGenerator : IVsSingleFileGenerator {
        public int DefaultExtension(out string pbstrDefaultExtension) {
            pbstrDefaultExtension = ".cs";
            return pbstrDefaultExtension.Length;
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents,
          string wszDefaultNamespace, IntPtr[] rgbOutputFileContents,
          out uint pcbOutput, IVsGeneratorProgress pGenerateProgress) {
            try {
                List<string> lines = new List<string>();
                var className = Path.GetFileNameWithoutExtension(wszInputFilePath);

                lines.Add($"namespace {wszDefaultNamespace}");
                lines.Add("{");
                lines.Add($"\tpublic class {className}");
                lines.Add("\t{");

                lines.Add(GenerateCulture());
                lines.Add(GenerateLocaleManager(className));

                var reader = new ResxReader(wszInputFilePath);
                foreach (var item in reader) {
                    lines.Add(string.Empty);
                    lines.Add(GenerateDescription(item.Value));
                    lines.Add(GenerateMethod(item.Name));
                }

                lines.Add("\t}");
                lines.Add("}");

                var result = string.Join("\n", lines);

                byte[] bytes = Encoding.UTF8.GetBytes(result);
                int length = bytes.Length;
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(length);
                Marshal.Copy(bytes, 0, rgbOutputFileContents[0], length);
                pcbOutput = (uint)length;
            } catch (Exception ex) {
                pcbOutput = 0;
            }
            return VSConstants.S_OK;
        }

        private static string Culture => nameof(Culture);
        private static string LocaleManager => nameof(LocaleManager);

        private string GenerateCulture() {
            return $"\t\tpublic static {nameof(System)}.{nameof(System.Globalization)}.{nameof(System.Globalization.CultureInfo)} {Culture} {{get; set;}}";
        }
        private string GenerateLocaleManager(string className) {
            return $"\t\tpublic static {nameof(MbyronModsCommon)}.{nameof(MbyronModsCommon.LocalizeManager)} {LocaleManager} {{get;}} = new {nameof(MbyronModsCommon)}.{nameof(MbyronModsCommon.LocalizeManager)}(\"{className}\", typeof({className}).{nameof(Type.Assembly)});";
        }

        private string GenerateDescription(string text) {
            if (string.IsNullOrEmpty(text))
                return "\t\t//";
            else {
                var parts = text.Split('\n');
                var partText = parts[0].Length <= 100 ? parts[0] : parts[0].Substring(0, 100);

                return $"\t\t/// <summary>\n\t\t/// {partText}\n\t\t/// </summary>";
            }
        }
        private string GenerateMethod(string key) {
            return $"\t\tpublic static {nameof(String).ToLower()} {key.Replace('-', '_')} => {LocaleManager}.GetString(\"{key}\", {Culture});";
        }
    }
}
