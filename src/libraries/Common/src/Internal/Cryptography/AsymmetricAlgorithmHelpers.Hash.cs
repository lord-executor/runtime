// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Internal.Cryptography
{
    //
    // Common infrastructure for AsymmetricAlgorithm-derived classes that layer on OpenSSL.
    //
    internal static partial class AsymmetricAlgorithmHelpers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5350", Justification = "SHA1 is used when the user asks for it.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5351", Justification = "MD5 is used when the user asks for it.")]
        public static byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
        {
            // The classes that call us are sealed and their base class has checked this already.
            Debug.Assert(data != null);
            Debug.Assert(count >= 0 && count <= data.Length);
            Debug.Assert(offset >= 0 && offset <= data.Length - count);
            Debug.Assert(!string.IsNullOrEmpty(hashAlgorithm.Name));

            ReadOnlySpan<byte> source = data.AsSpan(offset, count);

            return
                hashAlgorithm == HashAlgorithmName.SHA256 ? SHA256.HashData(source) :
                hashAlgorithm == HashAlgorithmName.SHA1 ? SHA1.HashData(source) :
                hashAlgorithm == HashAlgorithmName.SHA512 ? SHA512.HashData(source) :
                hashAlgorithm == HashAlgorithmName.SHA384 ? SHA384.HashData(source) :
                hashAlgorithm == HashAlgorithmName.MD5 ? MD5.HashData(source) :
                throw new CryptographicException(SR.Cryptography_UnknownHashAlgorithm, hashAlgorithm.Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5350", Justification = "SHA1 is used when the user asks for it.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5351", Justification = "MD5 is used when the user asks for it.")]
        public static byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
        {
            // The classes that call us are sealed and their base class has checked this already.
            Debug.Assert(data != null);
            Debug.Assert(!string.IsNullOrEmpty(hashAlgorithm.Name));

            return
                hashAlgorithm == HashAlgorithmName.SHA256 ? SHA256.HashData(data) :
                hashAlgorithm == HashAlgorithmName.SHA1 ? SHA1.HashData(data) :
                hashAlgorithm == HashAlgorithmName.SHA512 ? SHA512.HashData(data) :
                hashAlgorithm == HashAlgorithmName.SHA384 ? SHA384.HashData(data) :
                hashAlgorithm == HashAlgorithmName.MD5 ? MD5.HashData(data) :
                throw new CryptographicException(SR.Cryptography_UnknownHashAlgorithm, hashAlgorithm.Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5350", Justification = "SHA1 is used when the user asks for it.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA5351", Justification = "MD5 is used when the user asks for it.")]
        public static bool TryHashData(ReadOnlySpan<byte> source, Span<byte> destination, HashAlgorithmName hashAlgorithm, out int bytesWritten)
        {
            // The classes that call us are sealed and their base class has checked this already.
            Debug.Assert(!string.IsNullOrEmpty(hashAlgorithm.Name));

            return
                hashAlgorithm == HashAlgorithmName.SHA256 ? SHA256.TryHashData(source, destination, out bytesWritten) :
                hashAlgorithm == HashAlgorithmName.SHA1 ? SHA1.TryHashData(source, destination, out bytesWritten) :
                hashAlgorithm == HashAlgorithmName.SHA512 ? SHA512.TryHashData(source, destination, out bytesWritten) :
                hashAlgorithm == HashAlgorithmName.SHA384 ? SHA384.TryHashData(source, destination, out bytesWritten) :
                hashAlgorithm == HashAlgorithmName.MD5 ? MD5.TryHashData(source, destination, out bytesWritten) :
                throw new CryptographicException(SR.Cryptography_UnknownHashAlgorithm, hashAlgorithm.Name);
        }
    }
}
