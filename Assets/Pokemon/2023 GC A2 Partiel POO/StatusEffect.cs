
namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Enum des status de chaque attaque (voir plus bas)
    /// </summary>
    public enum StatusPotential { NONE, SLEEP, BURN, CRAZY }
    
    public class StatusEffect
    {
        Fight _fight;
        protected Character _character;

        /// <summary>
        /// Factory retournant un nouvel objet représentant le statut généré
        /// </summary>
        /// <param name="s">le statut à générer</param>
        /// <returns>Le statut à appliquer sur le character ciblé</returns>
        public static StatusEffect GetNewStatusEffect(StatusPotential s, Fight fight, Character character)
        {
            
            switch (s)
            {
                case StatusPotential.SLEEP:
                    return new SleepStatus(fight, character);
                case StatusPotential.BURN:
                    return new BurnStatus(fight, character);
                case StatusPotential.CRAZY:
                    return new CrazyStatus(fight, character);
                case StatusPotential.NONE:
                default:
                    return null;
            }
        }
        /// <summary>
        /// Un Status ne peut etre crée que par une classe enfant (voir plus bas)
        /// </summary>
        /// <param name="remainingTurn">Nombre de tour de l'effet</param>
        /// <param name="damageEachTurn">Nombre de dégât à la fin de chaque tour</param>
        /// <param name="canAttack">Le personnage peut-il attaquer ?</param>
        /// <param name="damageOnAttack">Portion de l'attaque auto-infligé au moment de l'attaque( 1f:100%, 0.5f:50% etc</param>
        protected StatusEffect(Fight fight, Character character,int remainingTurn=0, int damageEachTurn=0, bool canAttack=false, float damageOnAttack=0f)
        {
            RemainingTurn = remainingTurn;
            DamageEachTurn = damageEachTurn;
            DamageOnAttack = damageOnAttack;
            CanAttack = canAttack;
            _character = character;
            _fight = fight;
            _fight.EndTurn += EndTurn;
        }

        /// <summary>
        /// Nombre de tour de l'effet
        /// </summary>
        public int RemainingTurn { get; protected set; }
        /// <summary>
        /// Nombre de dégât à la fin de chaque tour
        /// </summary>
        public int DamageEachTurn { get; protected set; }
        /// <summary>
        /// Le personnage peut-il attaquer ?
        /// </summary>
        public bool CanAttack { get; protected set; }
        /// <summary>
        /// Portion de l'attaque auto-infligé au moment de l'attaque( 1f:100%, 0.5f:50% etc
        /// </summary>
        public float DamageOnAttack { get; protected set; }

        /// <summary>
        /// Méthode enclenché par le système de combat à la fin de chaque tour
        /// Vous pouvez ajouter du contenu si besoin
        /// </summary>
        public virtual void EndTurn()
        {
            RemainingTurn--;
            if(RemainingTurn <= 0) Destroy();
        }

        public virtual void Destroy(){
            _fight.EndTurn -= EndTurn;
            _character.CurrentStatus = null;
        }
    }



    /// <summary>
    /// Endormi, le personnage ne peut pas attaquer
    /// </summary>
    public class SleepStatus : StatusEffect
    {
        public SleepStatus(Fight fight, Character character) : base(fight, character, 5, 0, false, 0f)
        {
        }
    }

    /// <summary>
    /// Brûlé, le personnage perd des points de vie à la fin de chaque tour
    /// </summary>
    public class BurnStatus : StatusEffect
    {
        public BurnStatus(Fight fight, Character character) : base(fight, character,5, 10, true, 0f)
        {
        }

        public override void EndTurn()
        {
            _character.CurrentHealth -= DamageEachTurn;
            base.EndTurn();
        }
    }

    /// <summary>
    /// Folie, le personnage s'attaque contre-lui même (on skip la notion attaque-defense au profit d'une portion de la stat d'attaque
    /// </summary>
    public class CrazyStatus : StatusEffect
    {
        public CrazyStatus(Fight fight, Character character) : base(fight, character,1, 0, false, 0.3f)
        {
        }
    }

}
