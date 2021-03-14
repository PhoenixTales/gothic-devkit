using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.CSSParser
{
    /// <summary></summary>
    [Flags]
    public enum TagType
    {
        /// <summary></summary>
        Named = 1,
        /// <summary></summary>
        Classed = 2,
        /// <summary></summary>
        IDed = 4,
        /// <summary></summary>
        Pseudoed = 8
    }

    /// <summary></summary>
    public enum Unit
    {
        /// <summary></summary>
        EX,
        /// <summary></summary>
        EM,
        /// <summary></summary>
        PX,
        /// <summary></summary>
        CM,
        /// <summary></summary>
        MM,
        /// <summary></summary>
        PC,
        /// <summary></summary>
        IN,
        /// <summary></summary>
        PT
    }

    /// <summary></summary>
    public enum ValType
    {
        /// <summary></summary>
        String,
        /// <summary></summary>
        Hex,
        /// <summary></summary>
        Unit,
        /// <summary></summary>
        Percent,
        /// <summary></summary>
        Url
    }
}
