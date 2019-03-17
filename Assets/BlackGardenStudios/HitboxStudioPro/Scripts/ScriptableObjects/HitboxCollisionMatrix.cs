using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BlackGardenStudios.HitboxStudioPro
{
    //[CreateAssetMenu()] //Uncomment if you need to make a new one.
    public class HitboxCollisionMatrix : ScriptableObject
    {
        public enum EVENT
        {
            NONE,
            RECV,
            SEND,
            BOTH
        }

        public EVENT[] m_CollisionMatrix;

        static public HitboxCollisionMatrix Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (HitboxCollisionMatrix)Resources.Load("HitboxCollisionMatrix");
                    if (m_Instance == null)
                        Debug.LogError("Resources/HitboxCollisionMatrix is missing! Did you delete or move this object?");
                }

                return m_Instance;
            }
            private set
            {
                m_Instance = value;
            }
        }

        static private HitboxCollisionMatrix m_Instance;

        //Visual studio doesn't expose this but this will be called at the appropriate time.
        private void OnValidate()
        {
            var matrix = new List<EVENT>(m_CollisionMatrix);
            var values = Enum.GetValues(typeof(HitboxType));
            var len = values.Length;
            var matrixLen = len * len;

            //Really should search out the missing type and extract that value specifically.
            while (matrix.Count > matrixLen)
                matrix.RemoveAt(matrix.Count - 1);

            while (matrix.Count < matrixLen)
                matrix.Add(EVENT.NONE);

            m_CollisionMatrix = matrix.ToArray();
        }

        /// <summary>
        /// Test a contact pair to see which, if any hitbox owners receive an event.
        /// </summary>
        /// <returns>RECV = A gets event, SEND = B gets event</returns>
        static public EVENT TestPair(HitboxType A, HitboxType B)
        {
            return Instance._TestPair(A, B);
        }

        private EVENT _TestPair(HitboxType A, HitboxType B)
        {
            var len = Mathf.RoundToInt(Mathf.Sqrt(m_CollisionMatrix.Length));
            bool swap = B < A;
            //then reverse the x axis as is done in the inspector
            var x = len - (1 + (int)(swap ? A : B));
            //y axis always has to be the smaller type
            var y = (int)(swap ? B : A);
            var result = m_CollisionMatrix[y * len + x];

            if (swap)
            {
                if (result == EVENT.RECV)
                    result = EVENT.SEND;
                else if (result == EVENT.SEND)
                    result = EVENT.RECV;
            }

            return result;
        }
    }
}