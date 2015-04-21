using System;
using System.IO;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics
{
    #region DDClass

    public class DD
    {
        public static void Get(string inFile, string outFile, ulong offset, uint blockSize, uint count)
        { 
            // Get handle (hVolume) for inFile
            IntPtr hVolume = NativeMethods.getHandle(inFile);
            
            // Get FileStream for reading from the hVolume handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Open file for reading
                using (FileStream streamToWrite = new FileStream(outFile, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    for (int i = 0; i < count; i++)
                    {
                        // Read the block size amount of bytes from the Volume
                        byte[] buffer = NativeMethods.readDrive(streamToRead, offset, blockSize);

                        // Writes a block of bytes to this stream using data from a byte array.
                        streamToWrite.Write(buffer, 0, buffer.Length);

                        // Increment offset to read from
                        offset += blockSize;
                    }
                }
            }
        }
    }  

    #endregion DDClass
}
