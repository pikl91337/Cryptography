using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AsymmetricCypher
{
    /// <summary>
    /// A structure for keys holding
    /// </summary>
    public struct KeysHolder
    {
        /// <summary>
        /// Public key
        /// </summary>
        public BigInteger publicKey;

        /// <summary>
        /// Private key
        /// </summary>
        public BigInteger privateKey;

        /// <summary>
        /// Cypher module
        /// </summary>
        public BigInteger cypherModule;
    }
}
