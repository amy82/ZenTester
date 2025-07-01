using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Data
{
    public class _TaskData
    {
        public LOTDATA LotData;
        public PRODUCTION_INFO ProductionInfo;
        public int PintCount;
    }

    public class LOTDATA
    {
        public string BarcodeData { get; set; }
    }
    public class PRODUCTION_INFO
    {
        public int OkCount { get; set; }
        public int NgCount { get; set; }
        public int TotalCount { get; set; }
    }
    public class TaskDataYaml
    {
        public _TaskData TaskData { get; private set; }

        public bool TaskDataLoad()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTask);       //LOT DATA
            
            try
            {
                if (!File.Exists(filePath))
                {
                    TaskData = new _TaskData();
                    TaskData.LotData = new LOTDATA();
                    TaskData.ProductionInfo = new PRODUCTION_INFO();
                    return false;
                }


                TaskData = Data.YamlManager.LoadYaml<_TaskData>(filePath);
                if (TaskData == null)
                {

                    return false;
                }

                Globalo.dataManage.TaskWork.m_szChipID = TaskData.LotData.BarcodeData;
                Globalo.dataManage.TaskWork.Judge_Total_Count = TaskData.ProductionInfo.TotalCount;
                Globalo.dataManage.TaskWork.Judge_Ok_Count = TaskData.ProductionInfo.OkCount;
                Globalo.dataManage.TaskWork.Judge_Ng_Count = TaskData.ProductionInfo.NgCount;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskDataLoad: {ex.Message}");
                return false;
            }
        }
        public bool TaskDataSave()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTask);       //LOT DATA
            try
            {
                if (!File.Exists(filePath))
                    return false;

                Data.YamlManager.SaveYaml(filePath, TaskData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskDataSave: {ex.Message}");
                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------------
        //
        // AOI TEST
        //
        //
        //--------------------------------------------------------------------------------------------------------------
        public static bool Save_AoiConfig()
        {
            string fileName = "AoiConfig.yaml";
            string pgPath = "";
            if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            {
                pgPath = "AoiData";
            }
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, pgPath , Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentRecipe, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, Globalo.yamlManager.aoiRoiConfig);
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskDataSave: {ex.Message}");
                return false;
            }

        }
        public static Data.AoiRoiConfig Load_AoiConfig()
        {
            string pgPath = "";
            string fileName = "AoiConfig.yaml";
            if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            {
                pgPath = "AoiData";
            }
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, pgPath, Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentRecipe, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Data.AoiRoiConfig();
                }

                Data.AoiRoiConfig data = Data.YamlManager.LoadYaml<Data.AoiRoiConfig>(filePath);
                if (data == null)
                {
                    return new Data.AoiRoiConfig();
                }

                for (int i = 0; i < data.markData.Count; i++)
                {
                    data.markData[i].name = Enum.GetName(typeof(VisionClass.eMarkList), i);
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Transfer: {ex.Message}");
                return new Data.AoiRoiConfig();
            }
        }
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    }//end


}
