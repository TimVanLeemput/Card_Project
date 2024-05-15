using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace OpenAI_API.Models
{
	/// <summary>
	/// Represents a language model
	/// </summary>
	public class Model
	{
		/// <summary>
		/// The id/name of the model
		/// </summary>
		[JsonProperty("id")]
		public string ModelID { get; set; }

		/// <summary>
		/// The owner of this model.  Generally "openai" is a generic OpenAI model, or the organization if a custom or finetuned model.
		/// </summary>
		[JsonProperty("owned_by")]
		public string OwnedBy { get; set; }

		/// <summary>
		/// The type of object. Should always be 'model'.
		/// </summary>
		[JsonProperty("object")]
		public string Object { get; set; }

		/// The time when the model was created
		[JsonIgnore]
		public DateTime? Created => CreatedUnixTime.HasValue ? (DateTime?)(DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime.Value).DateTime) : null;

		/// <summary>
		/// The time when the model was created in unix epoch format
		/// </summary>
		[JsonProperty("created")]
		public long? CreatedUnixTime { get; set; }

		/// <summary>
		/// Permissions for use of the model
		/// </summary>
		[JsonProperty("permission")]
		public List<Permissions> Permission { get; set; } = new List<Permissions>();

		/// <summary>
		/// Currently (2023-01-27) seems like this is duplicate of <see cref="ModelID"/> but including for completeness.
		/// </summary>
		[JsonProperty("root")]
		public string Root { get; set; }

		/// <summary>
		/// Currently (2023-01-27) seems unused, probably intended for nesting of models in a later release
		/// </summary>
		[JsonProperty("parent")]
		public string Parent { get; set; }

		/// <summary>
		/// Allows an model to be implicitly cast to the string of its <see cref="ModelID"/>
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to cast to a string.</param>
		public static implicit operator string(Model model)
		{
			return model?.ModelID;
		}

		/// <summary>
		/// Allows a string to be implicitly cast as an <see cref="Model"/> with that <see cref="ModelID"/>
		/// </summary>
		/// <param name="name">The id/<see cref="ModelID"/> to use</param>
		public static implicit operator Model(string name)
		{
			return new Model(name);
		}

		/// <summary>
		/// Represents an Model with the given id/<see cref="ModelID"/>
		/// </summary>
		/// <param name="name">The id/<see cref="ModelID"/> to use.
		///	</param>
		public Model(string name)
		{
			this.ModelID = name;
		}

		/// <summary>
		/// Represents a generic Model/model
		/// </summary>
		public Model()
		{

		}

		/// <summary>
		/// The default model to use in requests if no other model is specified.
		/// </summary>
		public static Model DefaultModel { get; set; } = ChatGPT35_Turbo;


		/// <summary>
		/// DEPRECATED
		/// Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost
		/// </summary>
		


		/// <summary>
		/// Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003. Will be updated with the latest model iteration.
		/// </summary>
		public static Model ChatGPT35_Turbo => new Model("gpt-3.5-turbo") { OwnedBy = "openai" };
     
        public static Model ChatGPT4_Turbo => new Model("gpt-4-turbo") { OwnedBy = "openai" };
        /// <summary>
        /// Our most advanced, multimodal flagship model that’s cheaper and faster than GPT-4 Turbo. 
		/// Currently points to gpt-4o-2024-05-13.
        /// </summary>
        public static Model ChatGPT4o => new Model("gpt-4o") { OwnedBy = "openai" };

		/// <summary>
		/// Stable text moderation model that may provide lower accuracy compared to TextModerationLatest.
		/// OpenAI states they will provide advanced notice before updating this model.
		/// </summary>
		public static Model TextModerationStable => new Model("text-moderation-stable") { OwnedBy = "openai" };

		/// <summary>
		/// The latest text moderation model. This model will be automatically upgraded over time.
		/// </summary>
		public static Model TextModerationLatest => new Model("text-moderation-latest") { OwnedBy = "openai" };


		/// <summary>
		/// Gets more details about this Model from the API, specifically properties such as <see cref="OwnedBy"/> and permissions.
		/// </summary>
		/// <param name="api">An instance of the API with authentication in order to call the endpoint.</param>
		/// <returns>Asynchronously returns an Model with all relevant properties filled in</returns>
		public async Task<Model> RetrieveModelDetailsAsync(OpenAI_API.OpenAIAPI api)
		{
			return await api.Models.RetrieveModelDetailsAsync(this.ModelID);
		}
		/// <summary>
		/// use this method to add all AI models to the Models list.
		/// </summary>
		public static List<Model> PopulateModels()
		{
			List<Model> Models = new List<Model>
            {
                ChatGPT35_Turbo,
				ChatGPT4_Turbo,
                ChatGPT4o       
            };
                return Models;
        }
	}

	/// <summary>
	/// Permissions for using the model
	/// </summary>
	public class Permissions
	{
		/// <summary>
		/// Permission Id (not to be confused with ModelId)
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Object type, should always be 'model_permission'
		/// </summary>
		[JsonProperty("object")]
		public string Object { get; set; }

		/// The time when the permission was created
		[JsonIgnore]
		public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

		/// <summary>
		/// Unix timestamp for creation date/time
		/// </summary>
		[JsonProperty("created")]
		public long CreatedUnixTime { get; set; }

		/// <summary>
		/// Can the model be created?
		/// </summary>
		[JsonProperty("allow_create_engine")]
		public bool AllowCreateEngine { get; set; }

		/// <summary>
		/// Does the model support temperature sampling?
		/// https://beta.openai.com/docs/api-reference/completions/create#completions/create-temperature
		/// </summary>
		[JsonProperty("allow_sampling")]
		public bool AllowSampling { get; set; }

		/// <summary>
		/// Does the model support logprobs?
		/// https://beta.openai.com/docs/api-reference/completions/create#completions/create-logprobs
		/// </summary>
		[JsonProperty("allow_logprobs")]
		public bool AllowLogProbs { get; set; }

		/// <summary>
		/// Does the model support search indices?
		/// </summary>
		[JsonProperty("allow_search_indices")]
		public bool AllowSearchIndices { get; set; }

		[JsonProperty("allow_view")]
		public bool AllowView { get; set; }

		/// <summary>
		/// Does the model allow fine tuning?
		/// https://beta.openai.com/docs/api-reference/fine-tunes
		/// </summary>
		[JsonProperty("allow_fine_tuning")]
		public bool AllowFineTuning { get; set; }

		/// <summary>
		/// Is the model only allowed for a particular organization? May not be implemented yet.
		/// </summary>
		[JsonProperty("organization")]
		public string Organization { get; set; }

		/// <summary>
		/// Is the model part of a group? Seems not implemented yet. Always null.
		/// </summary>
		[JsonProperty("group")]
		public string Group { get; set; }

		[JsonProperty("is_blocking")]
		public bool IsBlocking { get; set; }
	}

}
