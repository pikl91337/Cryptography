using System;
using System.Collections.Generic;
using System.Text;

namespace StreamCypher
{
    /// <summary>
    /// Corelation Test
    /// </summary>
    public class CorelationTester
    {
        /// <summary>
        /// series length
        /// </summary>
        private int _K;

        /// <summary>
        /// bit sequence for testing
        /// </summary>
        private List<short> _M;

        /// <summary>
        /// shifted on k bit sequence
        /// </summary>
        private List<short> _MplusK;

        /// <summary>
        /// Rk
        /// </summary>
        public double _Rk;
        
        /// <summary>
        /// Rкр
        /// </summary>
        public double _Rkr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m">Sequence for testing</param>
        /// <param name="k">series length</param>
        public CorelationTester(List<short> m,int k)
        {
            _M = m;
            _K = k;
            _MplusK = new List<short>();
            FillMplusK();
            DeleteTailFromM();
        }

        public void Test()
        {
            int xiSum = 0;
            int xiPlusKSum = 0;
            for (int i = 0; i < _M.Count; i++)
            {
                xiSum += _M[i];
                xiPlusKSum += _MplusK[i];
            }
            double m = ((double)xiSum / (_M.Count - _K));
            double mk = ((double)xiPlusKSum / (_M.Count - _K));

            double tmp = 0;
            double tmp1 = 0;
            for (int i = 0; i < _M.Count; i++)
            {
                tmp += (_M[i] - m) * (_MplusK[i] - mk);
            }
            var e = tmp / (_M.Count - _K);

            tmp = 0;
            for (int i = 0; i < _M.Count; i++)
            {
                tmp += Math.Pow(_M[i] - m, 2);
                tmp1 += Math.Pow(_MplusK[i] - mk, 2);
            }

            var dxi = tmp / (_M.Count - _K - 1);
            var dxiPlusK = tmp1 / (_M.Count - _K - 1);

            var rk = e / Math.Sqrt(dxiPlusK * dxi);
            _Rk = rk;

            var temp = (double)(_M.Count - 3) / (_M.Count + 1);
            var temp1 = Math.Sqrt(_M.Count * temp);
            var temp2 = (2 * temp1) / (_M.Count - 2);
            var temp3 = (double) 1 / (_M.Count - 1);
            var rkr = temp2 + temp3;
            _Rkr = rkr;
        }

        /// <summary>
        /// Filling shifted sequence
        /// </summary>
        private void FillMplusK()
        {
            for (int i = _K; i < _M.Count; i++)
            {
                _MplusK.Add(_M[i]);
            }
        }

        /// <summary>
        /// Deleting a tail frim sequence
        /// </summary>
        private void DeleteTailFromM()
        {
            for (int i = 0;i < _K; i++)
            {
                _M.RemoveAt(_M.Count - 1);
            }
        }
    }
}
