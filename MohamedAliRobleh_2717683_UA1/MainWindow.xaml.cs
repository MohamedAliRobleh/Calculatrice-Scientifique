using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MohamedAliRobleh_2717683_UA1
{
    
    public partial class MainWindow : Window
    {
        double lastNumber, result;
        string selectedOperator;
        private bool isNewNumber = true;
        private List<string> operationList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Resultat.Content = "0"; // Toujours initialisé à 0
            ZoneSaisie.Content = ""; // ZoneSaisie vide pour l'affichage des entrées
        }

        // Gestion des nombres
        private void Button_Click_Number(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string value = button.Content.ToString();

                // Gérer l'entrée de nombre négatif
                if (value == "-" && isNewNumber)
                {
                    Resultat.Content = "-"; // Permettre de commencer avec un nombre négatif
                    isNewNumber = false;
                    return;
                }

                // Afficher le nombre dans ZoneSaisie
                ZoneSaisie.Content += value;

                if (isNewNumber)
                {
                    Resultat.Content = value;
                    isNewNumber = false;
                }
                else
                {
                    Resultat.Content += value;
                }
            }
        }

        // Gestion des opérateurs (+, -, *, ÷)
        private void OnOperatorClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string operatorValue = button.Content.ToString();

                // Si un nombre négatif suit un opérateur
                if (operatorValue == "-" && isNewNumber)
                {
                    Resultat.Content = "-";
                    isNewNumber = false;
                    return;
                }

                // Ajouter le dernier nombre à la liste des opérations
                operationList.Add(Resultat.Content.ToString());
                operationList.Add(operatorValue);

                // Afficher l'opérateur dans ZoneSaisie
                ZoneSaisie.Content += $" {operatorValue} ";

                Resultat.Content = "0";
                isNewNumber = true;
            }
        }


        // Gestion des fonctions trigonométriques
        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            // Si un opérateur trigonométrique est sélectionné, effectuez le calcul
            if (!string.IsNullOrEmpty(selectedOperator))
            {
                double operand = double.Parse(Resultat.Content.ToString());

                // Appliquer la fonction trigonométrique correspondante
                switch (selectedOperator)
                {
                    case "sin":
                        result = Math.Sin(operand * Math.PI / 180); // Convertir en radians
                        break;
                    case "cos":
                        result = Math.Cos(operand * Math.PI / 180); // Convertir en radians
                        break;
                    case "tan":
                        result = Math.Tan(operand * Math.PI / 180); // Convertir en radians
                        break;
                    case "arcsin":
                        result = Math.Asin(operand) * (180 / Math.PI); // Convertir en degrés
                        break;
                    case "arccos":
                        result = Math.Acos(operand) * (180 / Math.PI); // Convertir en degrés
                        break;
                    case "arctan":
                        result = Math.Atan(operand) * (180 / Math.PI); // Convertir en degrés
                        break;
                }

                // Afficher le résultat dans Resultat
                Resultat.Content = result.ToString();
                isNewNumber = true;

                // Réinitialiser l'opérateur trigonométrique
                selectedOperator = null;

                return; // Sortir de la fonction après avoir calculé la fonction trigonométrique
            }

            // Ajouter le dernier nombre à la liste des opérations
            operationList.Add(Resultat.Content.ToString());

            // Calculer le résultat en respectant la priorité des opérateurs
            result = CalculeDePriorités(operationList);

            // Afficher le résultat final dans Resultat
            Resultat.Content = result.ToString();
            isNewNumber = true;

            // Réinitialiser la liste d'opérations
            operationList.Clear();
        }


        // Fonction pour gérer la priorité des opérateurs
        private double CalculeDePriorités(List<string> operations)
        {
            // D'abord gérer les multiplications et divisions
            for (int i = 1; i < operations.Count; i++)
            {
                if (operations[i] == "*" || operations[i] == "÷")
                {
                    double operand1 = double.Parse(operations[i - 1]);
                    double operand2 = double.Parse(operations[i + 1]);
                    double partialResult = 0;

                    if (operations[i] == "*")
                        partialResult = operand1 * operand2;
                    else if (operations[i] == "÷")
                        partialResult = operand1 / operand2;

                    // Remplacer l'opération par le résultat dans la liste
                    operations[i - 1] = partialResult.ToString();
                    operations.RemoveAt(i); // Supprimer l'opérateur
                    operations.RemoveAt(i); // Supprimer l'opérande suivant
                    i--; // Reculer l'index pour réévaluer correctement
                }
            }

            // Ensuite gérer les additions et soustractions
            double result = double.Parse(operations[0]);
            for (int i = 1; i < operations.Count; i += 2)
            {
                string operatorValue = operations[i];
                double nextNumber = double.Parse(operations[i + 1]);

                if (operatorValue == "+")
                    result += nextNumber;
                else if (operatorValue == "-")
                    result -= nextNumber;
            }

            return result;
        }

        // Réinitialisation de la calculatrice
        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            Resultat.Content = "0"; // Toujours 0 après un clear
            ZoneSaisie.Content = ""; // Vider l'affichage des opérations
            lastNumber = 0;
            result = 0;
            isNewNumber = true;
            operationList.Clear();
        }

        // Effacement de l'opérande courante
        private void OnClearEntryClick(object sender, RoutedEventArgs e)
        {
            Resultat.Content = "0";
        }

        // Gestion des fonctions mathématiques (Sin, Cos, Tan, ArcSin, ArcCos, ArcTan)
        private void OnSin(object sender, RoutedEventArgs e)
        {
            selectedOperator = "sin";
            ZoneSaisie.Content = "sin("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        private void OnCos(object sender, RoutedEventArgs e)
        {
            selectedOperator = "cos";
            ZoneSaisie.Content = "cos("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        private void OnTan(object sender, RoutedEventArgs e)
        {
            selectedOperator = "tan";
            ZoneSaisie.Content = "tan("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        private void OnArcSin(object sender, RoutedEventArgs e)
        {
            selectedOperator = "arcsin";
            ZoneSaisie.Content = "arcsin("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        private void OnArcCos(object sender, RoutedEventArgs e)
        {
            selectedOperator = "arccos";
            ZoneSaisie.Content = "arccos("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        private void OnArcTan(object sender, RoutedEventArgs e)
        {
            selectedOperator = "arctan";
            ZoneSaisie.Content = "arctan("; // Affichage de l'opération
            Resultat.Content = "0";
            isNewNumber = true;
        }

        // Ajout d'un point décimal
        private void OnDecimalClick(object sender, RoutedEventArgs e)
        {
            if (!Resultat.Content.ToString().Contains("."))
            {
                Resultat.Content += ".";
                ZoneSaisie.Content += ".";
                isNewNumber = false;
            }
        }

        // Gestion du backspace
        private void OnBackspaceClick(object sender, RoutedEventArgs e)
        {
            if (Resultat.Content.ToString().Length > 0)
            {
                // Supprimer le dernier caractère dans Resultat
                Resultat.Content = Resultat.Content.ToString().Substring(0, Resultat.Content.ToString().Length - 1);

                // Si le champ est vide après la suppression, le remettre à zéro
                if (Resultat.Content.ToString().Length == 0)
                {
                    Resultat.Content = "0";
                }
            }
        }

        // Inverser le signe d'un nombre
        private void OnPlusMin(object sender, RoutedEventArgs e)
        {
            double tempNumber;
            if (double.TryParse(Resultat.Content.ToString(), out tempNumber))
            {
                tempNumber = -tempNumber; // Inverser le signe
                Resultat.Content = tempNumber.ToString();
                ZoneSaisie.Content = tempNumber.ToString();
            }
        }

        // Gestion des constantes mathématiques (Pi et e)
        private void OnPiClick(object sender, RoutedEventArgs e)
        {
            ZoneSaisie.Content += "π";
            Resultat.Content = Math.PI.ToString();
        }

        private void OnEClick(object sender, RoutedEventArgs e)
        {
            ZoneSaisie.Content += "e";
            Resultat.Content = Math.E.ToString();
        }
    }
}