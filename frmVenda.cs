﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Vendas
{
    public partial class frmVenda : Form
    {
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Aluno\\Desktop\\Vendas\\DbVenda.mdf;Integrated Security=True");
        public frmVenda()
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CarregaCbxProduto()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                string pro = "SELECT Id,nome FROM Produto ORDER BY nome";
                SqlCommand cmd = new SqlCommand(pro, con);
                con.Open();
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(pro, con);
                DataSet ds = new DataSet();
                da.Fill(ds, "produto");
                cbxProduto.ValueMember = "Id";
                cbxProduto.DisplayMember = "nome";
                cbxProduto.DataSource = ds.Tables["produto"];
                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void frmVenda_Load(object sender, EventArgs e)
        {
            CarregaCbxProduto();
            txtPreco.Enabled = false;
            txtQuantidade.Enabled = false;
            txtTotal.Enabled = false;
            btnAdicionar.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            btnVenda.Enabled = false;
			dgvVenda.Columns.Add("ID", "ID");
			dgvVenda.Columns.Add("Produto", "Produto");
			dgvVenda.Columns.Add("Quantidade", "Quantidade");
			dgvVenda.Columns.Add("Preco", "Preco");
			dgvVenda.Columns.Add("Total", "Total");
		}

		private void cbxProduto_SelectedIndexChanged(object sender, EventArgs e)
		{
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM produto WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", cbxProduto.SelectedValue);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtQuantidade.Enabled = true;
                    btnAdicionar.Enabled = true;
                    btnEditar.Enabled=true;
                    btnExcluir.Enabled=true;
                    txtPreco.Text = dr["preco"].ToString();
                    txtQuantidade.Focus();
                }
                dr.Close();
                con.Close();
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }
		}

		private void btnAdicionar_Click(object sender, EventArgs e)
		{
            if (txtQuantidade.Text == "")
            {
                MessageBox.Show("Por favor digite a quantidade do produto!", "quantidade", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantidade.Focus();
                return;
            }
           
            var repetido = false;
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                if (Convert.ToString(cbxProduto.SelectedValue) == Convert.ToString(dr.Cells[0].Value))
                {
                    repetido = true;
                }
            }
            if (repetido == false)
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dgvVenda);
                item.Cells[0].Value = cbxProduto.SelectedValue;
                item.Cells[1].Value = cbxProduto.Text;
                item.Cells[2].Value = txtQuantidade.Text;
                item.Cells[3].Value = txtPreco.Text;
                item.Cells[4].Value = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(txtPreco.Text);
                dgvVenda.Rows.Add(item);
                cbxProduto.Text = "";
                txtQuantidade.Text = string.Empty;
                txtPreco.Text = string.Empty;
                decimal soma = 0;
                foreach (DataGridViewRow dr in  dgvVenda.Rows)
                {
                    soma += Convert.ToDecimal(dr.Cells[4].Value);
                    txtTotal.Text = soma .ToString();
                }
            }
            else
            {
                MessageBox.Show("produto ja cadastrado!", "produto repetido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                      

		} 


	}
}
