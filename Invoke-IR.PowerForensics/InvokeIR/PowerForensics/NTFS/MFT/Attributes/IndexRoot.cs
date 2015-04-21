using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    #region IndexRootClass

    public class IndexRoot : Attr
    {

        #region Enums

        [FlagsAttribute]
        enum INDEX_ROOT_FLAGS
        {
            INDEX_ALLOCATION = 0x01
        }

        #endregion Enums

        #region Structs

        struct ATTR_INDEX_ROOT
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal uint AttributeType;
            internal uint CollationSortingRule;
            internal uint IndexSizeinBytes;
            internal byte IndexSizeinClusters;
            internal byte[] Padding;
            internal NODE_HEADER NodeHeader;

            internal ATTR_INDEX_ROOT(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                AttributeType = BitConverter.ToUInt32(bytes, 32);
                CollationSortingRule = BitConverter.ToUInt32(bytes, 36);
                IndexSizeinBytes = BitConverter.ToUInt32(bytes, 40);
                IndexSizeinClusters = bytes[44];
                Padding = bytes.Skip(45).Take(3).ToArray();
                NodeHeader = new NODE_HEADER(bytes.Skip(48).Take(16).ToArray());
            }
        }

        struct NODE_HEADER
        {
            internal uint StartEntryOffset;
            internal uint EndUsedOffset;
            internal uint EndAllocatedOffset;
            internal uint Flags;

            internal NODE_HEADER(byte[] bytes)
            {
                StartEntryOffset = BitConverter.ToUInt32(bytes, 0);
                EndUsedOffset = BitConverter.ToUInt32(bytes, 4);
                EndAllocatedOffset = BitConverter.ToUInt32(bytes, 8);
                Flags = BitConverter.ToUInt32(bytes, 12);
            }
        }

        #endregion Structs

        #region Properties

        public readonly string AttributeType;
        public readonly uint IndexSize;
        public readonly uint StartOffset;
        public readonly uint EndOffset;
        public readonly string Flags;
        public readonly uint IndexSizeinBytes;

        #endregion Properties

        #region Constructors

        internal IndexRoot(byte[] AttrBytes, string AttrName)
        {
            ATTR_INDEX_ROOT ir = new ATTR_INDEX_ROOT(AttrBytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), ir.header.commonHeader.ATTRType);
            NameString = AttrName;
            NonResident = ir.header.commonHeader.NonResident;
            AttributeId = ir.header.commonHeader.Id;
            AttributeType = Enum.GetName(typeof(ATTR_TYPE), ir.AttributeType);
            IndexSize = ir.IndexSizeinBytes;
            StartOffset = ir.NodeHeader.StartEntryOffset;
            EndOffset = ir.NodeHeader.EndUsedOffset;
            Flags = ((INDEX_ROOT_FLAGS)ir.NodeHeader.Flags).ToString();
        }

        #endregion Constuctors

    }

    #endregion IndexRootClass
}