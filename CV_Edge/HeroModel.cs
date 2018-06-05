using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 6fba5602-2416-4582-8f3b-931bc0eadc24_c97c9b55-21f5-48b8-9a25-83f235a0d8d7

namespace CV_Edge
{
    public sealed class HeroModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class HeroModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public HeroModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "Batman", float.NaN },
                { "Superman", float.NaN },
            };
        }
    }

    public sealed class HeroModel
    {
        private LearningModelPreview learningModel;
        public static async Task<HeroModel> CreateHeroModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            HeroModel model = new HeroModel();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<HeroModelOutput> EvaluateAsync(HeroModelInput input) {
            HeroModelOutput output = new HeroModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
