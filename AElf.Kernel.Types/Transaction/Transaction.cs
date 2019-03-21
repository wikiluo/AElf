using System;
using System.Security.Cryptography;
using Google.Protobuf;
using AElf.Common;
using Google.Protobuf.WellKnownTypes;

namespace AElf.Kernel
{
    public partial class Transaction
    {
        public Hash GetHash()
        {
            return Hash.FromRawBytes(GetSignatureData());
        }

        //TODO: Add case GetHashBytes [Case]
        public byte[] GetHashBytes()
        {
            return SHA256.Create().ComputeHash(GetSignatureData());
        }

        private byte[] GetSignatureData()
        {
            if (To == null)
            {
                throw new InvalidOperationException($"Transaction.To cannot be empty: {this}");
            }

            var txData = new Transaction
            {
                From = From.Clone(),
                To = To.Clone(),
                MethodName = MethodName
            };
            if (Params.Length != 0)
                txData.Params = Params;
            if (Time != null &&
                !Time.Equals(Timestamp.FromDateTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))))
                txData.Time = Time;
            txData.RefBlockNumber = RefBlockNumber;
            txData.RefBlockPrefix = RefBlockPrefix;
            return txData.ToByteArray();
        }

        public byte[] Serialize()
        {
            return this.ToByteArray();
        }
    }
}