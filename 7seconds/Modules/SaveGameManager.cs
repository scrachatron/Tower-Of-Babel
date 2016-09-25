using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Tower_Of_Babel
{
    static class Savegame
    {
        private static string filename = "Save";

        private static SaveFile SaveData;
        public static int LastSaveFile;
        

        public static void InitiateSave(int fileToSaveOver, SaveFile tempfile)
        {
            LastSaveFile = fileToSaveOver;
            filename = "Save " + fileToSaveOver + ".sav";
            SaveData = tempfile;
            //StorageDevice.BeginShowSelector(PlayerIndex.One, SaveToDevice, null);
            SaveData = new SaveFile();
        }
        public static void InitiateSave(SaveFile tempfile)
        {
            filename = "Save " + LastSaveFile + ".sav";
            SaveData = tempfile;
            //StorageDevice.BeginShowSelector(PlayerIndex.One, SaveToDevice, null);
            SaveData = new SaveFile();
        }

        private static void SaveToDevice(IAsyncResult result)
        {
            SaveFile data = SaveData;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            try
            {
                // Create an XML writer object with the settings we've specified.
                XmlWriter writer = XmlWriter.Create(filename, settings);
                //  We're using the Intermediate Serializer which is more useful (it saves dictionaries), but 
                //  limited to targetting PC only. There's other stuff you need to do to make this work. See the 
                // project notes on the Moodle.
                //IntermediateSerializer.Serialize<SaveFile>(writer, data, null);
                //writer.Close();
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static SaveFile InnitiateLoad(int filetoload)
        {
            LastSaveFile = filetoload;
            SaveData = new SaveFile();
            filename = "Save " + filetoload + ".sav";
            //StorageDevice.BeginShowSelector(PlayerIndex.One, LoadFromDevice, null);
            return SaveData;
        }
        private static void LoadFromDevice(IAsyncResult result)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            try
            {
                #region Checking wheather the file exists
                try
                {
                    XmlReader isalivecheck = XmlReader.Create(filename, settings);
                    isalivecheck.Close();
                }
                catch
                {

                    SaveData = new SaveFile();
                    return;
                }
                #endregion

                XmlReader reader = XmlReader.Create(filename, settings);
                try
                {
                    //SaveData = IntermediateSerializer.Deserialize<SaveFile>(reader, filename);
                }
                catch
                {
                    //UnauthorizedAccessException e

                    SaveData = new SaveFile();
                    reader.Close();
                    //Debug.WriteLine(e.Message);
                }
                reader.Close();
            }
            catch (UnauthorizedAccessException e)
            {
                SaveData = new SaveFile();
                Debug.WriteLine(e.Message);
            }
        }
    }

    [Serializable]
    public struct SaveFile
    {
        public string mapName;
        public List<int[,]> map;
        

        public SaveFile(string currM, List<int[,]> maps)
        {
            mapName = currM;
            map = maps;
        }
    }
}
