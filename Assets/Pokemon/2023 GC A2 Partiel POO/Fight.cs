
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        public event Action EndTurn;

        public Fight(Character character1, Character character2)
        {
            if(character1 is null || character2 is null){
                throw new ArgumentNullException();
            }
            Character1 = character1;
            Character2 = character2;
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished(){
            if(!Character1.IsAlive || !Character2.IsAlive) return true;
            return false;
        }

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            Character first, second;
            Skill firstSkill, secondSkill;
            if(Character1.Speed > Character2.Speed){
                first = Character1;
                firstSkill = skillFromCharacter1;
                second = Character2;
                secondSkill = skillFromCharacter2;
            }
            else if (Character1.Speed != Character2.Speed){
                first = Character2;
                firstSkill = skillFromCharacter2;
                second = Character1;
                secondSkill = skillFromCharacter1;
            }
            else {
                //Port Priority (choix random si j'ai le temps)
                first = Character1;
                firstSkill = skillFromCharacter1;
                second = Character2;
                secondSkill = skillFromCharacter2;
            }
            
            second.ReceiveAttack(firstSkill, this);
            if(second.IsAlive) first.ReceiveAttack(secondSkill, this);

            EndTurn?.Invoke();
        }

    }
}
