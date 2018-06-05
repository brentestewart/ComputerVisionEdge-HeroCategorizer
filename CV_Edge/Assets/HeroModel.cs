using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 6fba5602-2416-4582-8f3b-931bc0eadc24_c97c9b55-21f5-48b8-9a25-83f235a0d8d7

namespace CV_Edge
{
    public sealed class _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "Batman", float.NaN },
                { "Superman", float.NaN },
            };
        }
    }

    public sealed class _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7Model
    {
        private LearningModelPreview learningModel;
        public static async Task<_x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7Model> Create_x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7Model(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7Model model = new _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7Model();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<_x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelOutput> EvaluateAsync(_x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelInput input) {
            _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelOutput output = new _x0036_fba5602_x002D_2416_x002D_4582_x002D_8f3b_x002D_931bc0eadc24_c97c9b55_x002D_21f5_x002D_48b8_x002D_9a25_x002D_83f235a0d8d7ModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
