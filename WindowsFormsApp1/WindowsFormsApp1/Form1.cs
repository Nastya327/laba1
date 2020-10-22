using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
       private Assembly assembly;
        public int[] testArray= new int[] {1,2,3};

        public Form1()
        {
            InitializeComponent();
        }

        private string selectAssemblyFile()
        {
            openFileDialog1.Filter = "Dll files (*.dll)|*.dll|Exe files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog1.Title = "Select assembly file";
            return (openFileDialog1.ShowDialog() == DialogResult.OK) ? openFileDialog1.FileName : null;
        }
        private Assembly openAssembly(string path)
        {
            try
            {
                Assembly a = Assembly.LoadFrom(path);
                return a;
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить указанную сборку!",
                "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        void addRoot(TreeNode root, Type[] types)
        {
            TreeNode node = null;
            foreach (Type type in types)
            {
                node = new TreeNode();
                node.Text = type.ToString();
                //Если класс
                if (type.IsClass)
                {
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                //Если интерфейс
                else if (type.IsInterface)
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                //Если перечисление
                else if (type.IsEnum)
                {
                    node.ImageIndex = 3;
                    node.SelectedImageIndex = 3;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                else if (type.IsValueType && !type.IsPrimitive)
                {
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 4;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
                //Если массив
                else if (type.IsArray)
                {
                    node.ImageIndex = 7;
                    node.SelectedImageIndex = 7;
                    addFirstLevel(node, type);
                    root.Nodes.Add(node);
                }
            }
        }
        //Загрузить все поля, конструкторы и методы
        private void addFirstLevel(TreeNode node, Type type)
        {
            TreeNode node1 = null;
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            //Загрузить поля
            foreach (FieldInfo field in fields)
            {
                node1 = new TreeNode();
                node1.Text = field.FieldType.Name + " " + field.Name;
                node1.ImageIndex = 6;
                node1.SelectedImageIndex = 6;
                node.Nodes.Add(node1);
            }
            //Загрузить конструкторы
            foreach (ConstructorInfo constructor in constructors)
            {
                String s = "";
                ParameterInfo[] parametrs = constructor.GetParameters();
                foreach (ParameterInfo parametr in parametrs)
                {
                    s = s + parametr.ParameterType.Name + ", ";
                }
                s = s.Trim();
                s = s.TrimEnd(',');
                node1 = new TreeNode();
                node1.Text = node.Text + "(" + s + ")";
                node1.ImageIndex = 5;
                node1.SelectedImageIndex = 5;
                node.Nodes.Add(node1);
            }
            //Загрузить методы
            foreach (MethodInfo method in methods)
            {
                String s = "";
                ParameterInfo[] parametrs = method.GetParameters();
                foreach (ParameterInfo parametr in parametrs)
                {
                    s = s + parametr.ParameterType.Name + ", ";
                }
                s = s.Trim();
                s = s.TrimEnd(',');
                node1 = new TreeNode();
                node1.Text = method.ReturnType.Name + " " + method.Name + "("
                + s + ")";
                node1.ImageIndex = 4;
                node1.SelectedImageIndex = 4;
                node.Nodes.Add(node1);
            }
            foreach (FieldInfo array in fields)
            {
                node1 = new TreeNode();
                node1.Text = array.FieldType.Name + " " + array.Name;
                node1.ImageIndex = 7;
                node1.SelectedImageIndex = 7;
                node.Nodes.Add(node1);
            }
        }

        private void открытьСборкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            string path = selectAssemblyFile();
            if (path != null)
            {
                assembly = openAssembly(path);
            }
            if (assembly != null)
            {
                TreeNode root = new TreeNode();
                root.Text = assembly.GetName().Name;
                root.ImageIndex = 0;
                root.SelectedImageIndex = 0;
                treeView1.Nodes.Add(root);
                Type[] types = assembly.GetTypes();
                addRoot(root, types);
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
