using System.Numerics;

namespace AsymmetricCypher
{
    /// <summary>
    /// RSA
    /// </summary>
    public class RsaEncoder
    {
        /// <summary>
        /// RSA en(-de)cryption.
        /// </summary>
        /// <param name="Mi">block</param>
        /// <returns>BigInteger</returns>
        public BigInteger Encode(BigInteger Mi, BigInteger key, BigInteger mod)
        {
            return MathHelper.FastPow(Mi, key, mod);
        }


    }
}