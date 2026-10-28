using _77NeoWeb.prg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public static class Trdctr
{
    // Idioma actual (por defecto español)
    //ClsConexion Cnx = new ClsConexion();
    public static string Actual
    {
        get { return (HttpContext.Current.Session["Idioma"] as string) ?? "ES"; }
    }

    // Llama al SP y guarda el resultado en Session
    public static void Cargar(string idioma)
    {
        var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string  cnx= string.Format(ConfigurationManager.ConnectionStrings["PConexDBPpalPrmtr"].ConnectionString, "23.102.100.143", "DbConfigWeb", "sa", "Medellin2021**");
        using (var cn = new SqlConnection(cnx))
        using (var cmd = new SqlCommand("IdiomaALL", cn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodIdioma", idioma);         
            cn.Open();
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    string clave = dr["CodF"].ToString() + "|" + dr["Objeto"].ToString();
                    dic[clave] = dr["Texto"].ToString();
                }
            }
            cn.Close();
        }
        HttpContext.Current.Session["Idioma"] = idioma;
        HttpContext.Current.Session["DicIdioma"] = dic;
    }
    // Busca un texto puntual (útil para mensajes en el código C#)
    public static string Texto(string codF, string objeto, string porDefecto = "")
    {
        var dic = HttpContext.Current.Session["DicIdioma"] as Dictionary<string, string>;
        string t;
        if (dic != null && dic.TryGetValue(codF + "|" + objeto, out t)) return t;
        return porDefecto;
    }
    // Punto de entrada: traduce todo lo que cuelga de "padre"
    public static void Traducir(Control padre, params string[] codFs)
    {
         if (HttpContext.Current.Session["DicIdioma"] == null)
            Cargar(Actual);

        var dic = (Dictionary<string, string>)HttpContext.Current.Session["DicIdioma"];
        Recorrer(padre, codFs, dic);
    }
    // Busca el objeto en cada formulario, en orden, y devuelve el primero que exista
    static bool Buscar(Dictionary<string, string> dic, string[] codFs, string objeto, out string texto)
    {
        foreach (string f in codFs)
            if (dic.TryGetValue(f + "|" + objeto, out texto))
                return true;
        texto = null;
        return false;
    }
    static void Recorrer(Control padre, string[] codFs, Dictionary<string, string> dic)
    {
        foreach (Control c in padre.Controls)
        {
            string t;

            if (!string.IsNullOrEmpty(c.ID))
            {
                if (Buscar(dic, codFs, c.ID, out t))
                    AplicarTexto(c, t);

                if (Buscar(dic, codFs, c.ID + ".ToolTip", out t))
                {
                    var wc = c as WebControl;
                    if (wc != null) wc.ToolTip = t;
                }
            }

            var gv = c as GridView;
            if (gv != null) TraducirGrid(gv, codFs, dic);

            if (c.HasControls()) Recorrer(c, codFs, dic);
        }
    }
    static void AplicarTexto(Control c, string t)
    {
        if (c is ImageButton)
        {
            var ib = (ImageButton)c;
            ib.ToolTip = t;
            ib.AlternateText = t;
        }
        else if (c is Label) ((Label)c).Text = t;
        else if (c is Button) ((Button)c).Text = t;
        else if (c is LinkButton) ((LinkButton)c).Text = t;
        else if (c is CheckBox) ((CheckBox)c).Text = t;   // incluye RadioButton
        else if (c is HyperLink) ((HyperLink)c).Text = t;
        else if (c is Literal) ((Literal)c).Text = t;
    }
    static void TraducirGrid(GridView gv, string[] codFs, Dictionary<string, string> dic)
    {
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            var col = gv.Columns[i];
            if (string.IsNullOrEmpty(col.AccessibleHeaderText)) continue;

            string t;
            if (Buscar(dic, codFs, gv.ID + "." + col.AccessibleHeaderText, out t))
            {
                col.HeaderText = t;
                var tf = col as TemplateField;
                bool tieneHeaderTemplate = tf != null && tf.HeaderTemplate != null;
                if (gv.HeaderRow != null && i < gv.HeaderRow.Cells.Count && !tieneHeaderTemplate)
                    gv.HeaderRow.Cells[i].Text = t;
            }
        }
    }
    public static string CodFActual()
    {
        return System.IO.Path.GetFileNameWithoutExtension(
            HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
    }
    //***********************************************************************************************************
    public static string[] ListaCodF()
    {
        var lista = new List<string> { CodFActual() };
        var extra = HttpContext.Current.Items["CodFExtra"] as string[];
        if (extra != null) lista.AddRange(extra);
        lista.Add("GENERAL");
        return lista.ToArray();
    }
    public static string Mensaje(string objeto, params object[] args)
    {
        var dic = HttpContext.Current.Session["DicIdioma"] as Dictionary<string, string>;
        if (dic == null)
        {
            Cargar(Actual);
            dic = (Dictionary<string, string>)HttpContext.Current.Session["DicIdioma"];
        }

        string texto;
        if (!Buscar(dic, ListaCodF(), objeto, out texto))
            return "[" + objeto + "]";   // no existe: se nota a simple vista

        return (args != null && args.Length > 0) ? string.Format(texto, args) : texto;
    }
    //***********************************************************************************************************
    /*public static string Mensaje(string objeto, params object[] args)
    {
        var dic = HttpContext.Current.Session["DicIdioma"] as Dictionary<string, string>;
        if (dic == null)
        {
            Cargar(Actual);
            dic = (Dictionary<string, string>)HttpContext.Current.Session["DicIdioma"];
        }

        string texto;
        if (!dic.TryGetValue(CodFActual() + "|" + objeto, out texto) &&
            !dic.TryGetValue("GENERAL|" + objeto, out texto))
        {
            return "[" + objeto + "]";   // no existe: se ve a simple vista que falta
        }

        return (args != null && args.Length > 0) ? string.Format(texto, args) : texto;
    }*/
    // Muestra el mensaje como alerta del navegador
    public static void MostrarAlerta(Page pagina, string objeto, params object[] args)    {
        string msg = HttpUtility.JavaScriptStringEncode(Mensaje(objeto, args));
        ScriptManager.RegisterStartupScript(pagina, pagina.GetType(),
            "alerta" + Guid.NewGuid().ToString("N"),
            "alert('" + msg + "');", true);
    }    
    public static DataControlField BuscarColumna(GridView gv, string nombreColumna)
    {
        foreach (DataControlField c in gv.Columns)
            if (string.Equals(c.AccessibleHeaderText, nombreColumna, StringComparison.OrdinalIgnoreCase))
                return c;
        return null;
    }
}