using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        //upload
        public static string Upload(IFormFile file,string folderName)
        {
            //1)get located folder path
            string folderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",folderName);
            //2)get filename and make it unique
            string FileName =$"{Guid.NewGuid()}{file.FileName}";
            //3)get filepath(folder path+filename)
            string FilePath=Path.Combine(folderPath,FileName);
            //4)save file as stream
            using var FS=new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FS);
            //5)return file name
            return FileName;
        }
        //delete
        public static void Delete(string FileName,string FolderName)
        {
            //1)get filepath
            string FilePath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",FolderName,FileName);
            //2)check if file exist or not
            if (File.Exists(FilePath)) {
                //if exist remove it
                File.Delete(FilePath);
            }
        }
    }
}
