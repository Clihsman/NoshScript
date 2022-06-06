using NoshScript.Nosh.Collections;
using System;
using System.Collections.Generic;

namespace NoshScript.Nosh.Native.Types
{
    public sealed class NoshType
    {
        // Fields
        private string name;
        private NoshTypeCode type;
        private Package package;
        // Static Fields
        private static Dictionary<string, List<NoshType>> types = new Dictionary<string, List<NoshType>>();

        static NoshType()
        {
            Package package = new Package("nosh", null, null,null);
            NoshType m_null = new NoshType("null",NoshTypeCode.Null, package);
            NoshType m_target = new NoshType("target", NoshTypeCode.Target, package);
            NoshType m_method = new NoshType("method", NoshTypeCode.Target, package);
            NoshType m_object = new NoshType("object", NoshTypeCode.Object, package);

            NoshType n_int = new NoshType("int", NoshTypeCode.Int32, package);
            NoshType n_decimal = new NoshType("decimal", NoshTypeCode.Decimal, package);
        }

        // Constructor
        public NoshType(string name, NoshTypeCode type, Package package)
        {
            this.name = name;
            this.type = type;
            this.package = package;
            addType(package, this);
        }

        // Method Static
        private static void addType(Package package, NoshType type)
        {
            List<NoshType> noshTypes;
            if (!types.TryGetValue(package.getName(), out noshTypes))
            {
                noshTypes = new List<NoshType>();
                types.Add(package.getName(), noshTypes);
            }

            foreach (NoshType noshType in noshTypes)
                if (noshType.name == type.name)
                    throw new TypeAccessException(type.name);

            noshTypes.Add(type);
        }

        public static NoshType getType(string name) {
            string[] currentName = name.Split('.');
            string packageName = currentName[0].Trim();
            name = currentName[1].Trim();

            List<NoshType> noshTypes;
            if (!types.TryGetValue(packageName, out noshTypes))
                return null;

            foreach (NoshType noshType in noshTypes)
                if (noshType.name == name)
                    return noshType;

            return null;
        }

        public static NoshType getType(string name,Package[] packages)
        {
            foreach (Package package in packages)
            {
                NoshType type = getType(string.Format("{0}.{1}",package.getName(),name));
                if (type != null)
                    return type;
            }
            return null;
        }

        // Methods
        public string getName() {
            return string.Format("{0}.{1}",package.getName(),name);
        }

        public NoshTypeCode getTypeCode()
        {
            return type;
        }

        public Package getPackage()
        {
            return package;
        }

        // Method Override
        public override string ToString()
        {
            return string.Format("{0}.{1}",package.getName(),name);
        }
    }
}
