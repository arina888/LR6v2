using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private List<Tariff> tariffs = new List<Tariff>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                ShowErrorMessage("Введите название товара в поле \"Название тарифа\"");
                return;
            }

            if (!decimal.TryParse(priceTextBox.Text, out decimal price) || price <= 0)
            {
                ShowErrorMessage("Введите положительное число в поле \"Цена (руб/мин)\"");
                return;
            }

            double discount = 0;
            if (discountCheckBox.Checked)
            {
                if (!double.TryParse(discountTextBox.Text, out discount) || discount < 0 || discount > 100)
                {
                    ShowErrorMessage("Введите положительное число в поле \"Льготный тариф\"");
                    return;
                }
            }

            Price priceWithDiscount = new Price(price, discount);
            Price priceWithoutDiscount = new Price(price, 0);
            Tariff tariff = new Tariff(name, priceWithDiscount, priceWithoutDiscount);
            tariffs.Add(tariff);
            productsListBox.Items.Add(tariff.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tariffs.Count == 0)
            {
                ShowErrorMessage("Список пуст");
                return;
            }
            int  a = 0;
            double sum = 0;
            double rez = 0;
            Tariff cheapestProduct = tariffs[0];
            for (int i = 0; i < tariffs.Count; i++)
            {
                sum += (double)(tariffs[i].PriceWithDiscount.AmountWithDiscount);
                /*if (tariffs[i].PriceWithDiscount.AmountWithDiscount < cheapestProduct.PriceWithDiscount.AmountWithDiscount)
                {
                    cheapestProduct = tariffs[i];
                }*/
                a++;
            }
             rez = Math.Round(((double)sum / a),2);
           

            MessageBox.Show(rez.ToString());
            //MessageBox.Show($"Тариф с средней ценой: {cheapestProduct.Name} - цена: {cheapestProduct.PriceWithDiscount.AmountWithDiscount} руб/мин");
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if(number == ' ')
            {
                e.Handled = true;
            }
        }
    }

    public class Store
    {
        public Tariff Tariff { get; }

        public Tariff Tariff1
        {
            get => default;
            set
            {
            }
        }

        public Store(Tariff tariff)
        {
            Tariff = tariff;
        }
    }

    public class Tariff
    {
        public string Name { get; }
        public Price PriceWithDiscount { get; }
        public Price PriceWithoutDiscount { get; }

        public Price Price
        {
            get => default;
            set
            {
            }
        }

        public Tariff(string name, Price priceWithDiscount, Price priceWithoutDiscount)
        {
            Name = name;
            PriceWithDiscount = priceWithDiscount;
            PriceWithoutDiscount = priceWithoutDiscount;
        }

        public override string ToString()
        {
            return $"{Name} - цена по льготе: {PriceWithDiscount.AmountWithDiscount} руб.";
        }
    }

    public class Price : IComparable<Price>
    {
        public decimal Amount { get; }
        public double Discount { get; }

        public decimal AmountWithDiscount => Amount * (decimal)(1 - Discount / 100);

        public Price(decimal amount, double discount)
        {
            Amount = amount;
            Discount = discount;
        }

        public int CompareTo(Price other)
        {
            if (other == null)
            {
                return 1;
            }
            return AmountWithDiscount.CompareTo(other.AmountWithDiscount);
        }
    }

    public class Discount
    {
        public double Value { get; }

        public Discount(double value)
        {
            Value = value;
        }
    }

    public class DiscountedPrice : Price
    {
        public Discount Discount { get; }

        public DiscountedPrice(decimal amount, Discount discount)
            : base(amount, discount.Value)
        {
            Discount = discount;
        }
    }

    public class RegularPrice : Price
    {
        public RegularPrice(decimal amount)
         : base(amount, 0)
        {
        }
    }
}
