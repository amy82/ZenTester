using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Data
{
    public class _SecGemData
    {
        public string CurrentRecipe { get; set; }       //현재 사용중인 레시피 명
        public string CurrentModel { get; set; }       //현재 사용중인 모델 명
    }
    public class SecGemDataYaml
    {
        public _SecGemData ModelData { get; set; }

        public bool MesLoad()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathModelInfo);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                ModelData = Data.YamlManager.LoadYaml<_SecGemData>(filePath);
                if (ModelData == null)
                {
                    return false;
                }

                Globalo.dataManage.mesData.m_sMesPPID = ModelData.CurrentRecipe;        //yaml Load

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading MesLoad: {ex.Message}");
                return false;
            }
        }

        public bool MesSave()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathModelInfo);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                Data.YamlManager.SaveYaml(filePath, ModelData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Save YAML: {ex.Message}");
                return false;
            }
        }
        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            // 대상 폴더가 없으면 생성
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // 현재 폴더의 파일 복사
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, true); // 덮어쓰기 허용
            }

            // 하위 폴더 복사 (재귀적으로 수행)
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectory(subDir, destSubDir); // 재귀 호출
            }
        }
    }
    

}
