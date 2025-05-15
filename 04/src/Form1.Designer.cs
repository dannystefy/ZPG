using OpenTK.GLControl;
using OpenTK.Windowing.Common;

namespace ZPG
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            glControl = new GLControl();
            tabEditorMode = new TabControl();
            pageVertexEdit = new TabPage();
            listVertices = new ListBox();
            propertyVertex = new PropertyGrid();
            tabTriEdit = new TabPage();
            listTriangles = new ListBox();
            propertyTriangle = new PropertyGrid();
            button1 = new Button();
            tabEditorMode.SuspendLayout();
            pageVertexEdit.SuspendLayout();
            tabTriEdit.SuspendLayout();
            SuspendLayout();
            // 
            // glControl
            // 
            glControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            glControl.API = ContextAPI.OpenGL;
            glControl.APIVersion = new Version(3, 3, 0, 0);
            glControl.Flags = ContextFlags.Default;
            glControl.IsEventDriven = true;
            glControl.Location = new Point(8, 7);
            glControl.Margin = new Padding(2, 2, 2, 2);
            glControl.Name = "glControl";
            glControl.Profile = ContextProfile.Compatability;
            glControl.SharedContext = null;
            glControl.Size = new Size(548, 474);
            glControl.TabIndex = 0;
            glControl.Load += glControl_Load;
            glControl.Paint += glControl1_Paint;
            glControl.MouseClick += glControl_MouseClick;
            // 
            // tabEditorMode
            // 
            tabEditorMode.Controls.Add(pageVertexEdit);
            tabEditorMode.Controls.Add(tabTriEdit);
            tabEditorMode.Dock = DockStyle.Right;
            tabEditorMode.Location = new Point(561, 0);
            tabEditorMode.Margin = new Padding(2, 2, 2, 2);
            tabEditorMode.Name = "tabEditorMode";
            tabEditorMode.SelectedIndex = 0;
            tabEditorMode.Size = new Size(236, 488);
            tabEditorMode.TabIndex = 1;
            // 
            // pageVertexEdit
            // 
            pageVertexEdit.Controls.Add(listVertices);
            pageVertexEdit.Controls.Add(propertyVertex);
            pageVertexEdit.Location = new Point(4, 24);
            pageVertexEdit.Margin = new Padding(2, 2, 2, 2);
            pageVertexEdit.Name = "pageVertexEdit";
            pageVertexEdit.Padding = new Padding(2, 2, 2, 2);
            pageVertexEdit.Size = new Size(228, 460);
            pageVertexEdit.TabIndex = 0;
            pageVertexEdit.Text = "Body";
            pageVertexEdit.UseVisualStyleBackColor = true;
            // 
            // listVertices
            // 
            listVertices.Dock = DockStyle.Fill;
            listVertices.FormattingEnabled = true;
            listVertices.Location = new Point(2, 200);
            listVertices.Margin = new Padding(2, 2, 2, 2);
            listVertices.Name = "listVertices";
            listVertices.Size = new Size(224, 258);
            listVertices.TabIndex = 1;
            // 
            // propertyVertex
            // 
            propertyVertex.BackColor = SystemColors.Control;
            propertyVertex.Dock = DockStyle.Top;
            propertyVertex.Location = new Point(2, 2);
            propertyVertex.Margin = new Padding(2, 2, 2, 2);
            propertyVertex.Name = "propertyVertex";
            propertyVertex.Size = new Size(224, 198);
            propertyVertex.TabIndex = 0;
            // 
            // tabTriEdit
            // 
            tabTriEdit.Controls.Add(listTriangles);
            tabTriEdit.Controls.Add(propertyTriangle);
            tabTriEdit.Controls.Add(button1);
            tabTriEdit.Location = new Point(4, 24);
            tabTriEdit.Margin = new Padding(2, 2, 2, 2);
            tabTriEdit.Name = "tabTriEdit";
            tabTriEdit.Padding = new Padding(2, 2, 2, 2);
            tabTriEdit.Size = new Size(228, 460);
            tabTriEdit.TabIndex = 1;
            tabTriEdit.Text = "Trojúhelníky";
            tabTriEdit.UseVisualStyleBackColor = true;
            // 
            // listTriangles
            // 
            listTriangles.Dock = DockStyle.Fill;
            listTriangles.FormattingEnabled = true;
            listTriangles.Location = new Point(2, 192);
            listTriangles.Margin = new Padding(2, 2, 2, 2);
            listTriangles.Name = "listTriangles";
            listTriangles.Size = new Size(224, 246);
            listTriangles.TabIndex = 1;
            // 
            // propertyTriangle
            // 
            propertyTriangle.BackColor = SystemColors.Control;
            propertyTriangle.Dock = DockStyle.Top;
            propertyTriangle.Location = new Point(2, 2);
            propertyTriangle.Margin = new Padding(2, 2, 2, 2);
            propertyTriangle.Name = "propertyTriangle";
            propertyTriangle.Size = new Size(224, 190);
            propertyTriangle.TabIndex = 0;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Bottom;
            button1.Location = new Point(2, 438);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(224, 20);
            button1.TabIndex = 2;
            button1.Text = "Přidat";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonTriangleAdd;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(797, 488);
            Controls.Add(tabEditorMode);
            Controls.Add(glControl);
            Margin = new Padding(2, 2, 2, 2);
            Name = "Form1";
            Text = "Form1";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            tabEditorMode.ResumeLayout(false);
            pageVertexEdit.ResumeLayout(false);
            tabTriEdit.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabEditorMode;
        private TabPage pageVertexEdit;
        private ListBox listVertices;
        private PropertyGrid propertyVertex;
        public GLControl glControl;
        private TabPage tabTriEdit;
        private ListBox listTriangles;
        private PropertyGrid propertyTriangle;
        private Button button1;
    }
}
