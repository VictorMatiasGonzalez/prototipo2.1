
namespace Francos
{
    partial class modificarEnfermero
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblenfermero = new System.Windows.Forms.Label();
            this.txtNombreyApellido = new System.Windows.Forms.TextBox();
            this.btnAceptarMod = new System.Windows.Forms.Button();
            this.btncerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblenfermero
            // 
            this.lblenfermero.AutoSize = true;
            this.lblenfermero.Location = new System.Drawing.Point(13, 32);
            this.lblenfermero.Name = "lblenfermero";
            this.lblenfermero.Size = new System.Drawing.Size(95, 13);
            this.lblenfermero.TabIndex = 0;
            this.lblenfermero.Text = "Nombre y Apellido:";
            // 
            // txtNombreyApellido
            // 
            this.txtNombreyApellido.Location = new System.Drawing.Point(124, 29);
            this.txtNombreyApellido.Name = "txtNombreyApellido";
            this.txtNombreyApellido.Size = new System.Drawing.Size(147, 20);
            this.txtNombreyApellido.TabIndex = 1;
            // 
            // btnAceptarMod
            // 
            this.btnAceptarMod.Location = new System.Drawing.Point(52, 90);
            this.btnAceptarMod.Name = "btnAceptarMod";
            this.btnAceptarMod.Size = new System.Drawing.Size(75, 23);
            this.btnAceptarMod.TabIndex = 2;
            this.btnAceptarMod.Text = "Aceptar";
            this.btnAceptarMod.UseVisualStyleBackColor = true;
            this.btnAceptarMod.Click += new System.EventHandler(this.btnAceptarMod_Click);
            // 
            // btncerrar
            // 
            this.btncerrar.Location = new System.Drawing.Point(177, 90);
            this.btncerrar.Name = "btncerrar";
            this.btncerrar.Size = new System.Drawing.Size(75, 23);
            this.btncerrar.TabIndex = 3;
            this.btncerrar.Text = "Cerrar";
            this.btncerrar.UseVisualStyleBackColor = true;
            this.btncerrar.Click += new System.EventHandler(this.btncerrar_Click);
            // 
            // modificarEnfermero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 151);
            this.Controls.Add(this.btncerrar);
            this.Controls.Add(this.btnAceptarMod);
            this.Controls.Add(this.txtNombreyApellido);
            this.Controls.Add(this.lblenfermero);
            this.Name = "modificarEnfermero";
            this.Text = "Modificar";
            
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblenfermero;
        private System.Windows.Forms.TextBox txtNombreyApellido;
        private System.Windows.Forms.Button btnAceptarMod;
        private System.Windows.Forms.Button btncerrar;
    }
}