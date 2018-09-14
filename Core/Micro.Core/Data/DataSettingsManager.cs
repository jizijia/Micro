using Micro.Core.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Micro.Core.Data
{
    /// <summary>
    /// 数据连接字符串管理器
    /// </summary>
    public class DataSettingsManager
    {
        #region Const

        private const string DATA_SETTINGS_FILE_PATH = "~/dataSettings.json";

        #endregion

        #region Fields

        /// <summary>
        /// 文件提供者
        /// </summary>
        protected IAppFileProvider _fileProvider;

        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileProvider">文件提供者</param>
        public DataSettingsManager(IAppFileProvider fileProvider = null)
        {
            this._fileProvider = fileProvider ?? Defaults.DefaultFileProvider;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="filePath">File path; pass null to use default settings file path</param>
        /// <param name="reloadSettings">Indicates whether to reload data, if they already loaded</param>
        /// <returns>Data settings</returns>
        public virtual DataSettings LoadSettings(string filePath = null, bool reloadSettings = false)
        {
            if (!reloadSettings && Singleton<DataSettings>.Instance != null)
                return Singleton<DataSettings>.Instance;

            filePath = filePath ?? _fileProvider.MapPath(DATA_SETTINGS_FILE_PATH);

            //check whether file exists
            if (!_fileProvider.FileExists(filePath))
            {
                throw new CustomException("Connection string not configured");
            }
            var text = _fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new DataSettings();
            //get data settings from the JSON file
            Singleton<DataSettings>.Instance = JsonConvert.DeserializeObject<DataSettings>(text);
            return Singleton<DataSettings>.Instance;
        }

        /// <summary>
        /// Save settings to a file
        /// </summary>
        /// <param name="settings">Data settings</param>
        public virtual void SaveSettings(DataSettings settings)
        {
            Singleton<DataSettings>.Instance = settings ?? throw new ArgumentNullException(nameof(settings));

            var filePath = _fileProvider.MapPath(DATA_SETTINGS_FILE_PATH);

            //create file if not exists
            _fileProvider.CreateFile(filePath);

            //save data settings to the file
            var text = JsonConvert.SerializeObject(Singleton<DataSettings>.Instance, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

    }
}
