//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }
        public bool Cooked { get; private set; } = false;

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }


        public string GetTextToPrint()
        {
            if (this.FinalProduct == null)
            {
                throw new InvalidOperationException("FinalProduct cannot be null");
            }
            StringBuilder result = new StringBuilder($"Receta de {this.FinalProduct.Description}:\n");
            foreach (BaseStep step in this.steps)
            {
                result.AppendLine(step.GetTextToPrint());
            }

            result.AppendLine($"Costo de producción: {this.GetProductionCost()}");
            result.AppendLine($"Tiempo de Cocción: {this.GetCookTime()} minutos");
            result.AppendLine($"¿Está Cocido?: {this.Cooked}");

            return result.ToString();
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }
        public int GetCookTime()
        {
            int totalCookTime = 0;

            foreach (BaseStep step in this.steps)
            {
                // Verificamos si el paso es de tipo Step (que tiene un tiempo)
                if (step is Step)
                {
                    totalCookTime += ((Step)step).Time; // Sumamos el tiempo del Step al total
                }
                // Si es de tipo WaitStep, no afecta al tiempo total, ya que es un paso de espera
            }

            return totalCookTime;
        }

        public void Cook()
        {
            int cookTime = GetCookTime(); // Obtener el tiempo total de cocción

            Cooked = true; // Marcar la receta como cocida
        }
        public class RecipeXX : TimerClient
        {
            private Recipe recipe;
            public RecipeXX(Recipe recipe)
            {
                this.recipe = recipe;
            }
            public void TimeOut()
            {
                this.recipe.Cooked = true;
            }
        }

    }

}

