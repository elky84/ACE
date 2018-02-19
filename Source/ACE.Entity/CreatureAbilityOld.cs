using System;
using ACE.Entity.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ACE.Entity
{
    [Obsolete]
    public class CreatureAbilityOld : ICloneable, ICreatureXpSpendableStat
    {
        private AceObjectPropertiesAttribute _backer;

        [JsonProperty("ability")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Ability Ability
        {
            get { return (Ability)_backer.AttributeId; }
            protected set
            {
                _backer.AttributeId = (ushort)value;
            }
        }
        
        /// <summary>
        /// Returns the Base Value for a Creature's Ability, for Players this is set durring Character Creation 
        /// </summary>
        [JsonProperty("startingValue")]
        public uint StartingValue
        {
            get { return _backer.AttributeBase; }
            set
            {
                _backer.AttributeBase = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        /// <summary>
        /// Returns the Current Rank for a Creature's Ability
        /// </summary>
        [JsonProperty("ranks")]
        public uint Ranks
        {
            get { return _backer.AttributeRanks; }
            set
            {
                _backer.AttributeRanks = (ushort)value;
                _backer.IsDirty = true;
            }
        }

        [JsonIgnore]
        public uint Current
        {
            get { return Base; }
        }

        /// <summary>
        /// For Primary Abilities, Returns the Base Value Plus the Ranked Value
        /// For Secondary Abilities, Returns the adjusted Value depending on the current Abiliy formula
        /// </summary>
        [JsonProperty("baseValue")]
        public uint Base
        {
            get
            {
                // TODO: buffs?  not sure where they will go
                return this.Ranks + this.StartingValue;
            }
        }

        /// <summary>
        /// Returns the MaxValue of an ability, UnbuffedValue + Additional
        /// </summary>
        [JsonIgnore]
        public uint MaxValue
        {
            get
            {
                // TODO: once buffs are implemented, make sure we have a max Value wich calculates the buffs in, 
                // as it's needed for the UpdateHealth GameMessage. For now this is just the unbuffed value.

                return Base;
            }
        }

        /// <summary>
        /// Total Experience Spent on an ability
        /// </summary>
        [JsonProperty("experienceSpent")]
        public uint ExperienceSpent
        {
            get { return _backer.AttributeXpSpent; }
            set
            {
                _backer.AttributeXpSpent = value;
                _backer.IsDirty = true;
            }
        }

        public CreatureAbilityOld(Ability ability)
        {
            _backer = new AceObjectPropertiesAttribute();

            Ability = ability;
            StartingValue = 10;
        }

        public CreatureAbilityOld(AceObjectPropertiesAttribute attrib)
        {
            _backer = attrib;
        }

        public AceObjectPropertiesAttribute GetAttribute()
        {
            return _backer;
        }

        public void ClearDirtyFlags()
        {
            _backer.IsDirty = false;
            _backer.HasEverBeenSavedToDatabase = true;
        }

        public void SetDirtyFlags()
        {
            _backer.IsDirty = true;
            _backer.HasEverBeenSavedToDatabase = false;
        }

        public object Clone()
        {
            return new CreatureAbilityOld((AceObjectPropertiesAttribute)_backer.Clone());
        }
    }
}
