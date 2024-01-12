
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter
        
        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
            // - un heal ne régénère pas plus que les HP Max
            // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
            // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        [Test]
        public void TypeEffectiveness(){
            var bulbizarre = new Character(200, 10, 0, 10, TYPE.GRASS);

            var fireAtk = new FireBall(); //atk power 50
            var waterAtk = new WaterBlouBlou(); //atkpower20
            int oldHealth = bulbizarre.CurrentHealth;
            bulbizarre.ReceiveAttack(fireAtk);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(oldHealth - (int)(fireAtk.Power*1.2))); // dmg x1.2
            oldHealth = bulbizarre.CurrentHealth;
            bulbizarre.ReceiveAttack(waterAtk);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(oldHealth - (int)(waterAtk.Power*.8))); // dmg x.8

        }

        [Test]
        public void ApplyStatus(){
            var bulbizarre = new Character(100,10,10,10,TYPE.GRASS);
            var fb = new FireBall();
            bulbizarre.ReceiveAttack(fb);
            Assert.That(bulbizarre.CurrentStatus.GetType(), Is.EqualTo(new BurnStatus(new Fight(bulbizarre, bulbizarre), bulbizarre).GetType()));
        }

        [Test]
        public void BurnDoesDamageEachTurn(){
            var miaouss = new Character(1000,10,0,10,TYPE.NORMAL);
            var salameche = new Character(1000,10,10,10,TYPE.FIRE);
            Skill fb = new FireBall();
            Fight f = new Fight(miaouss, salameche);
            int oldHealth = miaouss.CurrentHealth;
            f.ExecuteTurn(fb,fb);
            Assert.That(miaouss.CurrentHealth, Is.EqualTo(oldHealth - fb.Power - new BurnStatus(new Fight(salameche, salameche), salameche).DamageEachTurn));
            oldHealth =miaouss.CurrentHealth;
            f.ExecuteTurn(fb,fb);
            Assert.That(miaouss.CurrentHealth, Is.EqualTo(oldHealth - fb.Power - new BurnStatus(new Fight(salameche, salameche), salameche).DamageEachTurn));

        }
    }
}
