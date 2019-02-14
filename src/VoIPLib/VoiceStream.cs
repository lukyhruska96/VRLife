using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIPLib
{
    /// <summary>
    /// Class providing modified voice sound from not only a microphone
    /// </summary>
    public class VoiceStream
    {

        private IHighLevelVoiceEffect[] HighLevelEffects { get; set; } = { };
        private ILowLevelVoiceEffect[] LowLevelEffects { get; set; } = { };

        private ISampleProvider _inputSampleProvider;

        private VoiceStreamStruct _voiceStreamStruct;

        private ISampleProvider _finalSampleProvider;

        private Action _highLevelProccessing;

        private Task _highLevelProccessingTask;

        /// <summary>
        /// Default constructor without filters
        /// </summary>
        public VoiceStream()
        {
            _inputSampleProvider = new MicrophoneStream().GetSampleProvider();
            Init();
        }

        /// <summary>
        /// Constructor with filters provided in an array
        /// </summary>
        /// <param name="lowLevelVoiceEffects"> The array of low level filters. </param>
        /// <param name="highLevelVoiceEffects">The array of high level filters. </param>
        public VoiceStream(ILowLevelVoiceEffect[] lowLevelVoiceEffects, IHighLevelVoiceEffect[] highLevelVoiceEffects)
        {
            _inputSampleProvider = new MicrophoneStream().GetSampleProvider();
            this.LowLevelEffects = lowLevelVoiceEffects;
            this.HighLevelEffects = highLevelVoiceEffects;
            Init();
        }

        /// <summary>
        /// Constructor with filters provided in a list
        /// </summary>
        /// <param name="lowLevelVoiceEffects"> The list of low level filters. </param>
        /// <param name="highLevelVoiceEffects">The list of high level filters. </param>
        public VoiceStream(List<ILowLevelVoiceEffect> lowLevelVoiceEffects, List<IHighLevelVoiceEffect> highLevelVoiceEffects)
        {
            _inputSampleProvider = new MicrophoneStream().GetSampleProvider();
            this.LowLevelEffects = lowLevelVoiceEffects.ToArray();
            this.HighLevelEffects = highLevelVoiceEffects.ToArray();
            Init();
        }

        /// <summary>
        /// Constructor with custom input source without filters
        /// </summary>
        /// <param name="customInput"> The custom input. </param>
        public VoiceStream(ISampleProvider customInput)
        {
            _inputSampleProvider = customInput;
            Init();
        }

        /// <summary>
        /// Constructor with custom input source with filters in an array
        /// </summary>
        /// <param name="customInput"> The custom input. </param>
        /// <param name="lowLevelVoiceEffects"> The array of low level filters. </param>
        /// <param name="highLevelVoiceEffects">The array of high level filters. </param>
        public VoiceStream(ISampleProvider customInput, ILowLevelVoiceEffect[] lowLevelVoiceEffects, IHighLevelVoiceEffect[] highLevelVoiceEffects)
        {
            _inputSampleProvider = customInput;
            this.LowLevelEffects = lowLevelVoiceEffects;
            this.HighLevelEffects = highLevelVoiceEffects;
            Init();
        }

        /// <summary>
        /// Constructor with custom input source with filters in an array
        /// </summary>
        /// <param name="customInput"> The custom input. </param>
        /// <param name="lowLevelVoiceEffects"> The list of low level filters. </param>
        /// <param name="highLevelVoiceEffects">The list of high level filters. </param>
        public VoiceStream(ISampleProvider customInput, List<ILowLevelVoiceEffect> lowLevelVoiceEffects, List<IHighLevelVoiceEffect> highLevelVoiceEffects)
        {
            _inputSampleProvider = customInput;
            this.LowLevelEffects = lowLevelVoiceEffects.ToArray();
            this.HighLevelEffects = highLevelVoiceEffects.ToArray();
            Init();
        }

        /// <summary>
        /// Init method, which creates a chain of filters with input provider as the first called element of the chain.
        /// </summary>
        private void Init()
        {
            // structure for high level filters
            _voiceStreamStruct = new VoiceStreamStruct();
            ISampleProvider sampleProvider = _inputSampleProvider;
            foreach (ILowLevelVoiceEffect effect in LowLevelEffects)
                sampleProvider = effect.GetSampleProvider(sampleProvider);
            sampleProvider = _voiceStreamStruct.GetSampleProvider(sampleProvider);
            _finalSampleProvider = sampleProvider;
            _highLevelProccessing = () =>
            {
                while (true)
                {
                    Task<VoiceStreamStruct> taskQueue = Task<VoiceStreamStruct>.FromResult(_voiceStreamStruct);
                    foreach (IHighLevelVoiceEffect effect in HighLevelEffects)
                    {
                        taskQueue = effect.ProccessAsync(taskQueue);
                    }
                }
            };
            // create task for infinite loop for the high level filters
            _highLevelProccessingTask = new Task(_highLevelProccessing, TaskCreationOptions.LongRunning);
        }

        public void Run()
        {
            // start the task for high level filters
            if (_highLevelProccessingTask.Status != TaskStatus.Running)
                _highLevelProccessingTask.Start();
        }

        public ISampleProvider GetSampleProvider()
        {
            return _finalSampleProvider;
        }
    }
}
