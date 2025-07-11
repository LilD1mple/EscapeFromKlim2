using EFK2.Game.FPSCounter.Interfaces;
using System;
using UnityEngine;

namespace EFK2.Game.FPSCounter
{
    public sealed class FPSCounterService : IFPSCounterService
    {
        private int _frameRange = 60;

        private int[] _fpsBuffer;
        private int _fpsBufferIndex;

        private int _averageFPS;

        private const int _bufferMaxSize = 1024;

        private void InitializeBuffer()
        {
            _fpsBuffer = new int[_frameRange];

            _fpsBufferIndex = 0;
        }

        private void UpdateBuffer()
        {
            _fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);

            if (_fpsBufferIndex >= _frameRange)
                _fpsBufferIndex = 0;
        }

        private void CalculateFps()
        {
            int sum = 0;

            for (int i = 0; i < _frameRange; i++)
            {
                int fps = _fpsBuffer[i];
                sum += fps;
            }

            _averageFPS = sum / _frameRange;
        }

        int IFPSCounterService.GetFPSCount()
        {
            if (_fpsBuffer == null || _frameRange != _fpsBuffer.Length)
                InitializeBuffer();

            UpdateBuffer();

            CalculateFps();

            return _averageFPS;
        }

        void IFPSCounterService.SetFPSLimit(int count)
        {
            if (count < -1)
                throw new ArgumentOutOfRangeException(nameof(count));

            _frameRange = count == -1 ? _bufferMaxSize : count;

            InitializeBuffer();
        }
    }
}
