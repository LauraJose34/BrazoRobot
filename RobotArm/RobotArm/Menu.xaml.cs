using RobotArm.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.WebRequestMethods;

namespace RobotArm
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Menu : ContentPage
	{
        string Motor, Tipo;
        int SumaAngulo = 90;
        List<Angulo> Lista;
        List<Angulo> MandarLista;
        RobotArmSender robotArmSender;
        

        public Menu ()
		{
			InitializeComponent ();
            Lista = new List<Angulo>();
            MandarLista = new List<Angulo>();
            robotArmSender = new RobotArmSender("192.168.4.1");
        }
        #region Metodos
        private void ActualizarTabla()
        {
            listValores.ItemsSource = null;
            listValores.ItemsSource = Lista;
        }

        private void LimpiarCampos()
        {
            txtValor.Text = string.Empty;            
        }

        private bool VerificarCampo(string motor, string valor)
        {
            int sumaTotal = 0;
            int numero ;

            foreach (var item in Lista)
            {
                numero = 0;
                if (item.Motor == motor)
                    if (item.Tipo == "-" || item.Tipo == "+")
                        numero = int.Parse(item.Tipo + item.Valor);
                    else {
                        numero = 0;
                        SumaAngulo = item.Valor;
                    }
                sumaTotal += numero;
            }
            return ((SumaAngulo + sumaTotal) + float.Parse(valor)) >= 0 && (SumaAngulo + sumaTotal) + float.Parse(valor) <= 180 ? true : false;
        }

        private int SumaMotor(string motor)
        {
            int suma = 0;
            foreach (var item in Lista)
            {
                if (item.Motor == motor)
                    suma += item.Valor;
            }
            return suma;
        }
        #endregion

        #region Controles Subir-Bajar

        private void BtnSubir_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Lista.Count > 0)
                {
                    var index = (listValores.ItemsSource as List<Angulo>).IndexOf(listValores.SelectedItem as Angulo);
                    if (index != 0)
                    {
                        Angulo angulo = listValores.SelectedItem as Angulo;
                        Lista.Remove(listValores.SelectedItem as Angulo);
                        Lista.Insert(index - 1, angulo);
                        listValores.SelectedItem = index - 1;
                    }
                    ActualizarTabla();
                }
            }
            catch (Exception)
            {
                DisplayAlert("Error!!", "No se puede subir el elemento", "Ok");
                return;
            }
        }

        private void BtnBajar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Lista.Count > 0)
                {
                    var index = (listValores.ItemsSource as List<Angulo>).IndexOf(listValores.SelectedItem as Angulo);
                    if (index != Lista.Count - 1)
                    {
                        Angulo angulo = listValores.SelectedItem as Angulo;
                        Lista.Remove(listValores.SelectedItem as Angulo);
                        Lista.Insert(index + 1, angulo);
                        listValores.SelectedItem = index + 1;
                    }
                }
                ActualizarTabla();
            }
            catch (Exception)
            {
                DisplayAlert("Error!!", "No se puede bajar el elemento", "Ok");
                return;
            }
        }
        private void CmbTipo_SelectedIndexChanged(object sender, EventArgs e) => Tipo = cmbTipo.Items[cmbTipo.SelectedIndex];

        private void CmbMotor_SelectedIndexChanged(object sender, EventArgs e) => Motor = cmbMotor.Items[cmbMotor.SelectedIndex];
        #endregion

        #region Botones
        private void BtnIngresar_Clicked(object sender, EventArgs e)
        {
            if (cmbTipo.SelectedIndex < 0)
            {
                DisplayAlert("Error!!!", "Falta seleccionar el Tipo de motor\nSeleccione uno", "OK");
                return;
            }
            if (cmbMotor.SelectedIndex < 0)
            {
                DisplayAlert("Error!!!", "Falta seleccionar un Motor\nSeleccione uno", "OK");
                return;
            }
            if (string.IsNullOrEmpty(txtValor.Text))
            {
                DisplayAlert("Error!!!", "Falta llenar el campo de Valor\nSeleccione uno", "OK");
                return;
            }
            if (int.TryParse(txtValor.Text, out int a) == false)
            {
                DisplayAlert("Error!!!", "El valor ingresado tiene que ser número", "OK");
                txtValor.Focus();
                return;
            }
            string numero = Tipo == "-" || Tipo == "+" ?  Tipo + txtValor.Text : txtValor.Text;
            if (VerificarCampo(Motor, numero.ToString()) == false)
            {
                DisplayAlert("Error!!!", "El angulo es demasiado grande!!\nTotal de angulo de motor: "+ SumaAngulo+"\nEl valor tiene que ser menor: " +(180-SumaAngulo), "OK");
                 return;
            }
            Angulo angulo = new Angulo();
            angulo.Motor = Motor;
            angulo.Valor = int.Parse(txtValor.Text);
            angulo.Tipo = Tipo;
            Lista.Add(angulo);
            ActualizarTabla();
            LimpiarCampos();
        }

        private void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Angulo angulo = listValores.SelectedItem as Angulo;
                if (angulo != null)
                {
                    cmbMotor.SelectedItem = angulo.Motor;
                    txtValor.Text = angulo.Valor.ToString();
                    cmbTipo.SelectedItem = angulo.Tipo;
                    Lista.Remove(angulo);
                }
            }
            catch (Exception)
            {
                DisplayAlert("Error!!!", "Vueleve a seleccionar", "OK");
                return;
            }
        }

        private void BtnNueva_Clicked(object sender, EventArgs e)
        {
            LimpiarCampos();
            Lista = new List<Angulo>();
            MandarLista = new List<Angulo>();
            Lista.Add(new Angulo
            {
                Motor = "A",
                Tipo = "+",
                Valor = 0,
            });
            Lista.Add(new Angulo
            {
                Motor = "B",
                Tipo = "+",
                Valor = 0,
            });
            Lista.Add(new Angulo
            {
                Motor = "C",
                Tipo = "+",
                Valor = 0,
            });
            Lista.Add(new Angulo
            {
                Motor = "D",
                Tipo = "+",
                Valor = 0,
            });
            Lista.Add(new Angulo
            {
                Motor = "E",
                Tipo = "+",
                Valor = 0,
            });
            Lista.Add(new Angulo
            {
                Motor = "F",
                Tipo = "+",
                Valor = 0,
            });
            listValores.ItemsSource = null;
            EmigrarLista();
            SumaAngulo = 0;
            robotArmSender.EnviarMovimientos(MandarLista);
            Lista = new List<Angulo>();
            MandarLista = new List<Angulo>();
        }

        private void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                Angulo angulo = listValores.SelectedItem as Angulo;
                Lista.Remove(angulo);
                DisplayAlert("Información", "El valor seleccionado ha sido eliminado correctamente ", "OK");
                ActualizarTabla();
            }
            catch (Exception)
            {
                DisplayAlert("Error!!!", "Vueleve a seleccionar", "OK");
                return;
            }
        }

        private void BtnEjecutar_Clicked(object sender, EventArgs e)
        {
            EmigrarLista();
            robotArmSender.EnviarMovimientos(MandarLista);
        }
        
        private void EmigrarLista()
        {
            int[] anguloInicial = new int[6] { 90, 90, 90, 90, 90, 90 };
            int Total = 0;
            int y = 0;
            foreach (var item in Lista)
            {
                int i = 0;
                foreach (var m in cmbMotor.Items)
                {
                    if (m.ToString() == item.Motor)
                    {
                        y = i;
                    }
                    i++;
                }
                int numAngulo = 0;
                if (item.Tipo == "Ninguno")
                {
                    anguloInicial[y] = item.Valor;
                    Total = anguloInicial[y];
                    numAngulo = 0;
                }
                else {
                    if (item.Tipo == "-")
                        numAngulo = -1 * item.Valor;                                
                    else
                        numAngulo = item.Valor;
                        anguloInicial[y] = anguloInicial[y] + numAngulo;
                     }

                Angulo NumAngulo = new Angulo();
                NumAngulo.Motor = item.Motor;
                NumAngulo.Valor = anguloInicial[y];
                NumAngulo.Tipo = item.Tipo;
                MandarLista.Add(NumAngulo);
            }
        }
        #endregion
    }
}