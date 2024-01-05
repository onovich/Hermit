using System;

namespace Polyperfect.War
{
    [Serializable]
    public struct IKWeight
    {
        public float Body, Head, Eye;

        public IKWeight(float body, float head, float eye)
        {
            Body = body;
            Head = head;
            Eye = eye;
        }
    }
}