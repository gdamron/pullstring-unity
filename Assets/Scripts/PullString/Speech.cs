//
// Assets/Scripts/PullString/Speech.cs
//
// Facilitate collection and processing of audio data for ASR.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PullString
{
    internal class Speech
    {
        public byte[] Bytes
        {
            get
            {
                return buffers.ToArray();
            }
        }
        private List<byte> buffers = new List<byte>();
        private bool isRecording;

        public void Start()
        {
            isRecording = true;
        }

        public void AddAudio(float[] buffer)
        {
            if (!isRecording || buffer == null || buffer.Length == 0)
            {
                return;
            }
            var bytes = ConvertSamplesToBytes(buffer);
            buffers.AddRange(bytes);
        }

        public void StreamAudio(float[] buffer, Stream stream)
        {
            if (!isRecording || buffer == null || buffer.Length == 0 || stream == null)
            {
                return;
            }
            var bytes = ConvertSamplesToBytes(buffer);
            stream.BeginWrite(bytes, 0, bytes.Length, (IAsyncResult asyncResult) => {
                var str = (Stream)asyncResult.AsyncState;
                str.EndWrite(asyncResult);
            }, stream);
        }

        public void Stop()
        {
            isRecording = false;
        }

        public void Flush()
        {
            buffers.Clear();
        }

        // Take an AudioClip of raw pcm data and convert it to bytes
        public static byte[] PrepareAudioData(AudioClip clip, int length = -1, int offset = 0)
        {
            if (clip.frequency != AudioContext.SampleRate ||
                clip.channels != AudioContext.Channels)
            {
                throw new Exception("Audio must be mono at 16000 sample rate.");
            }

            if (length < 0)
            {
                length = clip.samples;
            }

            var samples = new float[length];
            clip.GetData(samples, offset);
            return ConvertSamplesToBytes(samples);
        }

        private static byte[] ConvertSamplesToBytes(float[] samples)
        {
            var bytes = new byte[samples.Length * 2];
            int offset = 0;
            IntToByte i2b;
            foreach (var sample in samples)
            {
                var normalized = Mathf.Max(-1, Mathf.Min(1, sample));
                i2b.intVal = (ushort)(normalized < 0 ? normalized * 0x8000 : normalized * 0x7FFF);
                bytes[offset] = i2b.byte0;
                bytes[offset + 1] = i2b.byte1;
                offset += 2;
            }

            return bytes;
        }

        private struct IntToByte
        {
            public uint intVal;
            public byte byte0
            {
                get
                {
                    return (byte)intVal;
                }
            }
            public byte byte1
            {
                get
                {
                    return (byte)(intVal >> 8);
                }
            }
        }
    }

    internal class AudioContext
    {
        public static int SampleRate = 16000;
        public static int Channels = 1;
    }
}
