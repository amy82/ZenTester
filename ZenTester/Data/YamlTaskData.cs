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
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Transfer: {ex.Message}");
                return new Data.AoiRoiConfig();
            }
        }
        //
        //
        //

        //public static VisionClass.MarkDataGroup Load_MarkData(string Model, string fileName)
        //{
        //    string filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, Model, fileName);       //TRAY DATA
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //        {
        //            return new VisionClass.MarkDataGroup();
        //        }


        //        VisionClass.MarkDataGroup data = Data.YamlManager.LoadYaml<VisionClass.MarkDataGroup>(filePath);
        //        if (data == null)
        //        {

        //            return new VisionClass.MarkDataGroup();
        //        }
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error Load MarkData: {ex.Message}");
        //        return new VisionClass.MarkDataGroup();
        //    }
        //}
        //public static bool Save_MarkData(string Model, string fileName)
        //{
        //    string filePath = Path.Combine(CPath.BASE_AOI_DATA_PATH, Model, fileName);       //LOT DATA
        //    try
        //    {
        //        Data.YamlManager.SaveYaml(filePath, Globalo.visionManager.markUtil.markData);// data);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error Save MarkData: {ex.Message}");
        //        return false;
        //    }

        //}
        //--------------------------------------------------------------------------------------------------------------
        //
        // Transfer
        //
        //
        //--------------------------------------------------------------------------------------------------------------
        public static bool TaskSave_Transfer(Machine.PickedProduct data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskDataSave: {ex.Message}");
                return false;
            }

        }
        public static Machine.PickedProduct TaskLoad_Transfer(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.PickedProduct();
                }


                Machine.PickedProduct data = Data.YamlManager.LoadYaml<Machine.PickedProduct>(filePath);
                if (data == null)
                {

                    return new Machine.PickedProduct();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Transfer: {ex.Message}");
                return new Machine.PickedProduct();
            }
        }

        public static bool TaskSave_Layout(Machine.ProductLayout data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskSave_Layout: {ex.Message}");
                return false;
            }

        }
        public static Machine.ProductLayout TaskLoad_Layout(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.ProductLayout();
                }


                Machine.ProductLayout data = Data.YamlManager.LoadYaml<Machine.ProductLayout>(filePath);
                if (data == null)
                {

                    return new Machine.ProductLayout();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad_Layout Transfer: {ex.Message}");
                return new Machine.ProductLayout();
            }
        }
        


        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    }//end


}
