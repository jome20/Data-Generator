using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Data_Generator.Datalayer
{
 public    class export
    {
        #region directory
        static private string _dirMain;

        static public string dirMain
        {
            get
            {
                if (string.IsNullOrEmpty(_dirMain))
                    _dirMain = System.Windows.Forms.Application.StartupPath;
                return _dirMain;
            }
        }

        static string MVCPath { get { return dirMain + "\\MVC\\"; } }
        static string LaravelPath { get { return dirMain + "\\Laravel\\"; } }
        static string Laravel_Controller_Path { get { return LaravelPath + "Controller\\"; } }
        static string Laravel_Migration_Path { get { return LaravelPath + "Migration\\"; } }
        static string Laravel_Model_Path { get { return LaravelPath + "Models\\"; } }
        static string Laravel_Factory_Path { get { return LaravelPath + "Factory\\"; } }

        #endregion

        public static void exportData(List<string> TABLES)
        {
            //RepositoryMaker.setdatabase(dbname);

            string q = "show TABLES";
            //string q = "show TABLES";

            System.IO.Directory.CreateDirectory(LaravelPath);
            System.IO.Directory.CreateDirectory(MVCPath);
            System.IO.Directory.CreateDirectory(Laravel_Controller_Path);
            System.IO.Directory.CreateDirectory(Laravel_Model_Path);
            System.IO.Directory.CreateDirectory(Laravel_Factory_Path);

            System.IO.Directory.CreateDirectory(Laravel_Migration_Path);



            StreamWriter data_php = new StreamWriter(MVCPath + "Dataphp.php");
            StreamWriter repository_php = new StreamWriter(MVCPath + "Repository_php.php");

            //StreamWriter laravelMigration = new StreamWriter(LaravelPath+ "Migration.php");
            StreamWriter SwlaravelRotesAPI = new StreamWriter(LaravelPath + "RotesAPI.php");
            StreamWriter SwlaravelTeminal = new StreamWriter(LaravelPath + "Teminal.php");
            string RotesAPIDown = "\n";
            //StreamWriter controller = new StreamWriter(LaravelPath+ "controller.php");
            //StreamWriter laravel = new StreamWriter(LaravelPath+ "laravel.php");
            //StreamWriter laravel = new StreamWriter(LaravelPath+ "laravel.php");
            //StreamWriter laravel = new StreamWriter(LaravelPath + "laravel.php");

            System.IO.StreamWriter data1_part = new System.IO.StreamWriter("Data.cs");
            System.IO.StreamWriter dataHelp = new System.IO.StreamWriter("DataHelp.cs");
            System.IO.StreamWriter dataDart = new System.IO.StreamWriter("Datadart.dart");
            System.IO.StreamWriter repositoryDart = new System.IO.StreamWriter("repositoryDart.dart");
            System.IO.StreamWriter repositoryDartFix = new System.IO.StreamWriter("repositoryFix.dart");
            System.IO.StreamWriter _repository = new System.IO.StreamWriter("Repository.cs");
            System.IO.StreamWriter sqlitetsqll = new System.IO.StreamWriter("sqlitet.sql");
            System.IO.StreamWriter repositorysqlite = new System.IO.StreamWriter("Repositorysqlite.cs");

            data_php.WriteLine("<?php ");
            repository_php.WriteLine("<?php ");
            repository_php.WriteLine("class Repository{");
            string fncNameAll = "";
            repositoryDartFix.WriteLine("//#region ");
            var TNot = new string[] { "failed_jobs", "migrations", "password_resets", "personal_access_tokens", "users", "0000", "0000", };
            TNot = TNot.Select(p => p.ToLower()).ToArray();
            using (IDbConnection db = RepositoryMaker.GetIDbConnection())
            {
                // List<string> TABLES = db.Query<string>(q).ToList();
                foreach (var _TABLE in TABLES)
                {


                    string table = _TABLE[0].ToString().ToUpper() + _TABLE.Substring(1);
                    if (TNot.Contains(table.ToLower())) continue;

                    //StreamWriter laravelRotesAPI = new StreamWriter(LaravelPath + "RotesAPI.php");
                    //StreamWriter SwlaravelMigration = new StreamWriter(Laravel_Model_Path + table + ".php");
                    //RecorderController
                    StreamWriter SwlaravelController = new StreamWriter(Laravel_Controller_Path + table + "Controller" + ".php");
                    SwlaravelController.WriteLine(@"<?php
namespace App\Http\Controllers;

use App\Models\" + table + @";
use Illuminate\Http\Request;

class " + table + @"Controller extends Controller
{");
                    #region MyRegion
                    StreamWriter SwlaravelMigration = new StreamWriter(Laravel_Migration_Path + table + ".php");
                    SwlaravelMigration.WriteLine(@"<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class Create" + table + @"sTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('" + table + @"s', function (Blueprint $table) {");
                    //StreamWriter Swlaravel = new StreamWriter(LaravelPath + "RotesAPI.php");
                    #endregion

                    #region Factory 
                    StreamWriter SwlaravelFactory = new StreamWriter(Laravel_Factory_Path + table + ".php");
                    SwlaravelFactory.WriteLine(@"<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;
use Illuminate\Support\Str;

class " + table + @"Factory extends Factory
{
    /**
     * Define the model's default state.
     *
     * @return array
     */
    public function definition()
    {  // $this->faker->name(),
        return [");
                    #endregion

                    #region Model 

                    StreamWriter Swlaravel_Model = new StreamWriter(Laravel_Model_Path + table + ".php");
                    Swlaravel_Model.WriteLine(@"<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class " + table + @" extends Model
{
    use HasFactory;

    protected $fillable = [");
                    //Schema::create('" + table + @"s', function (Blueprint $table) {");
                    //StreamWriter Swlaravel = new StreamWriter(LaravelPath + "RotesAPI.php");
                    #endregion

                    #region foreach (var _TABLE in TABLES)

                    SwlaravelRotesAPI.WriteLine(@"use App\Http\Controllers\" + table + "Controller;");

                    //SwlaravelTeminal.WriteLine(@"php artisan make:model create_" + table + "_table -m");
                    SwlaravelTeminal.WriteLine(@"php artisan make:model " + table + " -m \n");
                    SwlaravelTeminal.WriteLine(@"php artisan make:controller " + table + "Controller --api \n");
                    SwlaravelTeminal.WriteLine(@"php artisan make:factory " + table + "Factory \n");
                    //UserFactory

                    RotesAPIDown += @"Route::resource('/" + table + "', " + table + "Controller::class );\n";

                    data1_part.WriteLine("public partial class " + table + "{");
                    data_php.WriteLine();
                    data_php.WriteLine("class " + table + "{");

                    dataHelp.WriteLine("public partial class " + table + "{ }");
                    dataDart.WriteLine("class " + table + "{");
                    q = "DESCRIBE `" + table + "`";
                    string insertstr = "";
                    string insertstrsqlite = "";
                    string insertstrvalue = "";
                    string insertstrvaluesqlite = "";
                    string editstr = "";
                    string editstrSqlite = "";
                    string editstrwhere = "";
                    string editstrwhereSqlite = "";

                    List<info> Tinfo = db.Query<info>(q).ToList();
                    string IDField = "";
                    string dartConstractor_ = "";
                    string strcolumnsDart = "";
                    string DartPart = "";
                    string dartGet = "";
                    string dartSet = "";
                    string fromMapDart = "";
                    string fromMapObjectDart = "";
                    var toMap_ = "";
                    var toMap_ID = "";
                    var toMap_widthID = "";

                    string help = createSQLiteTable(table, Tinfo);
                    sqlitetsqll.WriteLine(help);
                    //help = "CREATE TABLE "+table+ " (id integer  primary key ,  idsubject integer,  word varchar(50)  ,  mean varchar(200)  ,  example varchar(200) )";
                    var __construct = "\tfunction __construct($" + table + ")\n\t{\n";

                    #region SwlaravelController
                    SwlaravelController.WriteLine(@"    public function index()
    {
        return " + table + @"::All();
    }
    public function show($id)
    {
        return " + table + @"::find($id);
    }
    public function update(Request $request, $id)
    {
        $find = " + table + @"::find($id);
        $find->update($request->all());
        return $find;
    }
    public function destroy($id)
    {
        return " + table + @"::destroy($id);
    }
    public function store(Request $request)
    {
        $request -> validate([");
                    #endregion



                    foreach (var infoitem in Tinfo)
                    {
                        #region infoitem
                        if (infoitem.Field == "updated_at") continue;
                        if (infoitem.Field == "created_at") continue;

                        writeControllerField(SwlaravelController, infoitem);
                        writeMigrationField(SwlaravelMigration, infoitem);
                        writeModelField(Swlaravel_Model, infoitem);

                        writeFactoryField(SwlaravelFactory, infoitem);

                        #region Wite in C#
                        string defaultv = "";


                        string typeStr = TypeMysqlToCStype(infoitem.Type, ref defaultv);
                        data1_part.Write("public " + typeStr + " ");
                        data1_part.Write(infoitem.Field + "{ get; set; }");
                        data1_part.WriteLine();

                        data_php.WriteLine("\tvar $" + infoitem.Field + ";");

                        __construct += "\t\t$this->" + infoitem.Field + " = $" + table + "['" + infoitem.Field + "'];\n";
                        #endregion
                        #region Write in Dart
                        var Type = "";
                        typeStr = typeStr.Trim();
                        switch (typeStr)
                        {
                            default:
                                Type = TypeMysqlToCStype(infoitem.Type, ref defaultv) + " ";
                                break;
                        }

                        //llllllllllllllllllll;
                        DartPart += "late " + Type;
                        dartGet += Type + " get " + infoitem.Field + " => _" + infoitem.Field + ";\n";
                        dartSet += "\n  set " + infoitem.Field + @"(" + Type + @" newVal) {
      this." + infoitem.Field + "= newVal; }\n";

                        DartPart += "" + infoitem.Field;
                        DartPart += ";\n";
                        defaultv = "";
                        dartConstractor_ += ",this." + infoitem.Field + defaultv;
                        strcolumnsDart += ",'" + infoitem.Field + "'";

                        fromMapDart += "\n data['" + infoitem.Field + "'],";

                        var fieldVal_toMap = infoitem.Field;
                        var fieldVal_FromtoMap = "map['" + infoitem.Field + "']";


                        if (infoitem.Type.ToLower().StartsWith("date"))
                        {
                            //DateToSringFull
                            if (infoitem.Type.ToLower().StartsWith("datetime"))
                                fieldVal_toMap = "DateToSringFull(" + infoitem.Field + ")";
                            else
                                fieldVal_toMap = "DateToSring(" + infoitem.Field + ")";
                            fieldVal_FromtoMap = " DateTime.parse(" + fieldVal_FromtoMap + ")";
                        }
                        fromMapObjectDart += "     this." + infoitem.Field + " = " + fieldVal_FromtoMap + ";\n";

                        string toMapItem = "'" + infoitem.Field + "': " + fieldVal_toMap + ",\n";

                        if (infoitem.Key == "")
                            toMap_widthID += toMapItem;
                        else
                        {
                            toMap_ID += "if (" + infoitem.Field + "> 0)";
                        }

                        toMap_ += toMapItem;

                        #endregion
                        #region Primary Key
                        if (infoitem.Key == "PRI")
                        {
                            IDField = infoitem.Field;
                            editstrwhere = "where (" + IDField + " =@" + IDField + ")";
                            editstrwhereSqlite = "where (" + IDField + " ='\"+item." + IDField + "+\"')";
                        }
                        else
                        {
                            insertstr += ", " + infoitem.Field;
                            insertstrsqlite += ", " + infoitem.Field;
                            insertstrvalue += ", @" + infoitem.Field + "";
                            insertstrvaluesqlite += ",'\"+item." + infoitem.Field + "+\"'";
                            editstr += ", " + infoitem.Field + " = @" + infoitem.Field;
                            editstrSqlite += ", " + infoitem.Field + " ='\"+item." + infoitem.Field + "+\"'";
                        }
                        #endregion


                        #endregion
                    }

                    SwlaravelController.WriteLine(@"        ]);
        return " + table + @"::create($request->all());
    }");
                    __construct += "\t}\n";
                    data_php.WriteLine(__construct);
                    #region Dart
                    //factory Product.fromMap(Map<String, dynamic> data) {


                    dataDart.WriteLine(DartPart);

                    dataDart.WriteLine("static final columns = [ " + strcolumnsDart.Substring(1) + "];");
                    dartConstractor_ = table + "(" + dartConstractor_.Substring(1) + ");";


                    dataDart.WriteLine(dartConstractor_);
                    //dataDart.WriteLine(dartGet);
                    //dataDart.WriteLine(dartSet);



                    dataDart.WriteLine("   factory " + table + ".fromMap(Map<String, dynamic> data) {  return " + table + "( " + fromMapDart + ");\n}");
                    if (toMap_ID == "")
                        dataDart.WriteLine("Map<String, dynamic> toMap() { return {" + toMap_ + "};}");
                    else
                    {
                        dataDart.WriteLine("Map<String, dynamic> toMap() {  " + toMap_ID + " return {" + toMap_ + "}; ");
                        dataDart.WriteLine(" return {" + toMap_widthID + "};}");

                    }



                    dataDart.WriteLine(table + ".fromMapObject(Map<String, dynamic> map) {\n" + fromMapObjectDart + "}\n");
                    #endregion
                    _repository.WriteLine("#region " + table);
                    repository_php.WriteLine("// <editor-fold defaultstate='" + table + "' >");
                    repositorysqlite.WriteLine("#region " + table);
                    repositoryDart.WriteLine("//#region " + table);
                    if (IDField != "")
                    {
                        setrepositoryLst(_repository, table, IDField);
                        setrepositoryLst(repositorysqlite, table, IDField);
                        setrepositoryDartLst(repositoryDart, table, IDField);
                        setrepositoryDartLstFix(repositoryDartFix, table, IDField, ref fncNameAll);
                    }
                    #region insert
                    {
                        writeInseertPart(_repository, table, insertstr, insertstrvalue, IDField);
                        writeInseertPartDar(repositoryDart, table, insertstr, insertstrvalue, IDField);
                        writeInseertPartsqlite(repositorysqlite, table, insertstrsqlite, insertstrvaluesqlite, IDField);
                    }
                    #endregion
                    #region update
                    if (IDField != "")
                    {
                        setupdatePart(_repository, table, editstr, editstrwhere);
                        setupdatePart(repositorysqlite, table, editstrSqlite, editstrwhereSqlite);
                        setupdatePartDart(repositoryDart, table, editstr, editstrwhere);


                        //setupdatePart(repositorysqlite, TABLE, editstr, editstrwhere);
                    }

                    #endregion
                    #region save
                    if (IDField != "")
                    {
                        setsave(_repository, table, IDField);
                        setsave(repositorysqlite, table, IDField);
                    }
                    #endregion method list
                    #region list
                    {
                        //repository_php.WriteLine(
                        setLstRepositoryPHP(repository_php, table);

                        setLstRepository(_repository, table);
                        setLstRepository(repositorysqlite, table);
                    }
                    #endregion
                    #region list val
                    {
                        setLstVal(_repository, table);
                        setLstVal(repositorysqlite, table);
                    }
                    #endregion
                    #region Remove 
                    if (editstrwhere != "")
                    {
                        setRemove(_repository, table, editstrwhere);
                        //setRemove(repositorysqlite, TABLE, editstrwhere);
                    }
                    #endregion
                    _repository.WriteLine("#endregion");
                    repository_php.WriteLine("// </editor-fold>");


                    repositorysqlite.WriteLine("#endregion");
                    repositoryDart.WriteLine("//#endregion");
                    dataDart.WriteLine("}");
                    data1_part.WriteLine("}");

                    data_php.WriteLine("}");


                    // 
                    #endregion foreach (var _TABLE in TABLES)


                    SwlaravelMigration.WriteLine(@"            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('" + table + @"s');
    }
}");
                    SwlaravelMigration.Close();
                    SwlaravelController.WriteLine("}");

                    SwlaravelController.Close();

                    Swlaravel_Model.WriteLine("    ];\n}");
                    Swlaravel_Model.Close();


                    SwlaravelFactory.WriteLine(@"        ];
    }
}");
                    SwlaravelFactory.Close();
                }
            }


            repositoryDartFix.WriteLine("//#endregion");
            repositoryDartFix.WriteLine(" static void setFix() {");
            repositoryDartFix.WriteLine(fncNameAll);
            repositoryDartFix.WriteLine("  }");

            repositoryDartFix.Close();
            _repository.Close();
            repositorysqlite.Close();
            repositoryDart.Close();
            data1_part.Close();
            SwlaravelRotesAPI.WriteLine(RotesAPIDown);


            repository_php.WriteLine("}");
            repository_php.WriteLine("?>");
            repository_php.Close();
            data_php.WriteLine("?>");
            data_php.Close();

            dataHelp.Close();
            dataDart.Close();
            sqlitetsqll.Close();

            SwlaravelTeminal.Close();
            SwlaravelRotesAPI.Close();
        }

        private static void writeFactoryField(StreamWriter swlaravelFactory, info infoitem)
        {
            if (infoitem.Null == "YES") return;
            swlaravelFactory.WriteLine("            '" + infoitem.Field + "' => '' , ");
        }

        private static void writeModelField(StreamWriter swlaravelModel, info infoitem)
        {
            if (infoitem.Null == "YES") return;
            swlaravelModel.WriteLine("        '" + infoitem.Field + "' ,");
        }

        private static void writeControllerField(StreamWriter swlaravelController, info infoitem)
        {
            if (infoitem.Null == "YES") return;
            swlaravelController.WriteLine("            '" + infoitem.Field + "'=> 'required',");
        }

        private static void writeMigrationField(StreamWriter swlaravelMigration, info infoitem)
        {
            if (infoitem.Key != "")
            { }
            if (infoitem.Type.Contains("int"))
            {
                swlaravelMigration.WriteLine("            $table->integer('" + infoitem.Field + "');");
                return;
            }
            if (infoitem.Type.Contains("varchar"))
            {
                swlaravelMigration.WriteLine("            $table->string('" + infoitem.Field + "');");
                return;
            }

            if (infoitem.Type == "text")
            {
                swlaravelMigration.WriteLine("            $table->text('" + infoitem.Field + "');");
                return;
            }

            if (infoitem.Type.StartsWith("date") | infoitem.Type.Contains("time"))
            {
                swlaravelMigration.WriteLine("            $table->date('" + infoitem.Field + "');");
                return;
            }

            switch (infoitem.Type)
            {
                default:
                    break;
            }
        }

        private static void setLstRepositoryPHP(StreamWriter repositoryphp, string table) //TABLE  table
        {
            string _Lst_Table = table + "_lst";

            repositoryphp.WriteLine("\tprivate static $" + _Lst_Table + ";");
            repositoryphp.WriteLine("\t" + "public static function get" + _Lst_Table + "()");
            repositoryphp.WriteLine("\t{");

            repositoryphp.WriteLine("\t\tif (!self::$" + _Lst_Table + ")");

            repositoryphp.WriteLine("\t\t{");
            repositoryphp.WriteLine("\t\t\t" + "self::$" + _Lst_Table + " = Array();");
            repositoryphp.WriteLine("\t\t\t" + "global $db;");
            repositoryphp.WriteLine("\t\t\t" + "$query = 'SELECT * FROM " + table + "';");

            repositoryphp.WriteLine("\t\t\t" + "$statement = $db->prepare($query);");
            repositoryphp.WriteLine("\t\t\t" + "$statement->execute();");
            repositoryphp.WriteLine("\t\t\t" + "$res = $statement->fetchAll();");
            repositoryphp.WriteLine("\t\t\t" + "for ($i = 0; $i < count($res); $i++) {");
            repositoryphp.WriteLine("\t\t\t\t" + "$item = new " + table + "($res[$i]);");
            repositoryphp.WriteLine("\t\t\t\t" + "self::$" + _Lst_Table + "[$item->id] = $item;");
            repositoryphp.WriteLine("\t\t\t" + "}");
            repositoryphp.WriteLine("\t\t" + "}");
            repositoryphp.WriteLine("\t\t" + "return self::$" + _Lst_Table + ";");
            repositoryphp.WriteLine("\t" + "}");
        }

        private static string createSQLiteTable(string table, List<info> Tinfo)
        {
            string help = "";
            foreach (var infoitem in Tinfo)
            {

                var Type = TypeSqlitetype(infoitem.Type);
                help += "," + infoitem.Field + " " + Type;
                if (infoitem.Key != "") help += " primary key ";
            }
            help = "CREATE TABLE " + table + " (" + help.Substring(1) + ")";
            return help;
        }

        private static void setrepositoryDartLstFix(StreamWriter repositoryDart, string table, string iDField, ref string fncNameAll)
        {
            string fncName = "    set" + table + "ListFix()";
            fncNameAll += fncName + ";\n";
            string tableDart = setLowerFirst(table);
            var fixLst = "_" + table + "List";
            repositoryDart.WriteLine("   static late List<" + table + ">" + fixLst + ";");
            repositoryDart.WriteLine("   static List<" + table + "> get " + fixLst.Substring(1) + " {return " + fixLst + "; }");
            //   static  List<Word> get WordList  {return _WordList;}

            repositoryDart.WriteLine("   static Future<List<" + table + ">> " + fncName + " async {\n");
            repositoryDart.WriteLine("         //if(" + fixLst + " == null)");
            repositoryDart.WriteLine("         {");
            repositoryDart.WriteLine("           var " + tableDart + "MapList = await getMapList('" + tableDart + "'); // Get 'Map List' from database");
            repositoryDart.WriteLine("           int count = " + tableDart + "MapList.length;   ");
            repositoryDart.WriteLine("           List<" + table + "> " + tableDart + "List = [];");
            repositoryDart.WriteLine("           for (int i = 0; i < count; i++) {");
            repositoryDart.WriteLine("              " + tableDart + "List.add(" + table + ".fromMapObject(" + tableDart + "MapList[i]));");
            repositoryDart.WriteLine("           }");
            repositoryDart.WriteLine("         " + fixLst + " = " + tableDart + "List;");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         return " + fixLst + ";");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         ");


        }

        private static string setLowerFirst(string table)
        {
            return table[0].ToString().ToLower() + table.Substring(1);
        }

        private static void setupdatePartDart(StreamWriter repositoryDart, string table, string editstr, string editstrwhere)
        {

            string tableDart = setLowerFirst(table);

            repositoryDart.WriteLine("         Future<int> update" + table + "(" + table + " " + tableDart + ") async {");
            repositoryDart.WriteLine("         var db = await database;");
            repositoryDart.WriteLine("         var result = await db.update('" + table + "_table', " + tableDart + ".toMap(), where: 'id = ?', whereArgs: [" + tableDart + ".id]);");
            repositoryDart.WriteLine("         return result;");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         ");

            // repositoryDart.WriteLine("         0000");
        }

        private static void writeInseertPartDar(StreamWriter repositoryDart, string table, string insertstr, string insertstrvalue, string iDField)
        {
            string tableDart = setLowerFirst(table);

            repositoryDart.WriteLine("         Future<int> insert" + table + "(" + table + " " + tableDart + ") async {");
            repositoryDart.WriteLine("         Database db = await database;");
            repositoryDart.WriteLine("         var result = await db.insert('" + table + "_table', " + tableDart + ".toMap());");
            repositoryDart.WriteLine("         return result;");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         ");
        }


        private static void setrepositoryDartLst(StreamWriter repositoryDart, string table, string iDField)
        {
            string tableDart = setLowerFirst(table);

            repositoryDart.WriteLine("   Future<List<" + table + ">> get" + table + "List() async {\n");
            repositoryDart.WriteLine("         var " + tableDart + "MapList = await getMapList('" + tableDart + "'); // Get 'Map List' from database");
            repositoryDart.WriteLine("         int count = " + tableDart + "MapList.length;   ");
            repositoryDart.WriteLine("         List<" + table + "> " + tableDart + "List = [];");
            repositoryDart.WriteLine("         for (int i = 0; i < count; i++) {");
            repositoryDart.WriteLine("            " + tableDart + "List.add(" + table + ".fromMapObject(" + tableDart + "MapList[i]));");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         return " + tableDart + "List;");
            repositoryDart.WriteLine("         }");
            repositoryDart.WriteLine("         ");
            //throw new NotImplementedException();
        }

        private static void setRemove(System.IO.StreamWriter repository, string TABLE, string editstrwhere)
        {
            string Lst_Table = TABLE + "_lst";
            // DeletedRowInaccessibleException;
            string qmain;
            repository.WriteLine("public static void " + TABLE + "_delete(" + TABLE + " item)");
            repository.WriteLine("{");
            repository.WriteLine("string Message;");
            //repository.WriteLine(" if (!Permission.Check_Permission(\"delete\", iduser, \"" + TABLE + "\", out Message)) return;");
            repository.WriteLine("using (IDbConnection db = GetIDbConnection())");
            repository.WriteLine("{");
            qmain = "delete from  " + TABLE + " " + editstrwhere;// DELETE FROM `emp` WHERE `emp`.`id` = 9
            repository.WriteLine("string q =\" " + qmain + "\";");
            repository.WriteLine("db.Execute(q, item);");
            repository.Write("if (_" + Lst_Table + " != null)");
            repository.WriteLine("_" + Lst_Table + " .Remove (item);");
            // TODO: saveUserActivity
            //repository.WriteLine("Permission.saveUserActivity(\"delete\", iduser, \"" + TABLE + "\", out Message, item." + IDField + ");");
            repository.WriteLine("}");
            repository.WriteLine("}");
        }

        private static void setLstVal(System.IO.StreamWriter repository, string TABLE)
        {
            string Lst_Table = TABLE + "_lst";
            repository.WriteLine("static List<" + TABLE + "> _" + Lst_Table + ";");
            repository.WriteLine("public static List<" + TABLE + "> " + Lst_Table);
            repository.WriteLine("{");
            repository.WriteLine("get");
            repository.WriteLine("{");
            repository.Write("if (_" + Lst_Table + " == null)");
            repository.WriteLine(Lst_Table + "_m();");
            repository.WriteLine("return _" + Lst_Table + ";");
            repository.WriteLine("}");
            repository.WriteLine("}");
        }

        private static void setLstRepository(System.IO.StreamWriter repository, string TABLE)
        {
            string qmain;
            string Lst_Table = TABLE + "_lst";

            repository.WriteLine("public static List<" + TABLE + "> " + Lst_Table + "_m()");
            repository.WriteLine("{");

            repository.WriteLine("using (IDbConnection db = GetIDbConnection())");
            repository.WriteLine("{");

            qmain = "select * from  " + TABLE + "";
            repository.WriteLine("string q =\" " + qmain + "\";");

            repository.WriteLine("_" + Lst_Table + " = db.Query<" + TABLE + ">(q).ToList();");

            repository.WriteLine("return _" + Lst_Table + " ;");
            repository.WriteLine("}");
            repository.WriteLine("}");
        }

        private static void setsave(System.IO.StreamWriter repository, string TABLE, string IDField)
        {
            repository.WriteLine("public static void " + TABLE + "_save(" + TABLE + " item)");
            repository.WriteLine("{");
            repository.WriteLine("if (item." + IDField + " == 0 )");
            repository.WriteLine(TABLE + "_insert(item);");
            repository.WriteLine("else ");
            repository.WriteLine(TABLE + "_update(item);");
            repository.WriteLine("}");
        }

        private static void setupdatePart(System.IO.StreamWriter repository, string TABLE, string editstr, string editstrwhere)
        {
            string qmain;
            repository.WriteLine("public static void " + TABLE + "_update(" + TABLE + " item)");
            repository.WriteLine("{");
            repository.WriteLine("string Message;");
            //repository.WriteLine(" if (!Permission.Check_Permission(\"update\", iduser, \"" + TABLE + "\", out Message)) return;");
            // TODO:ddddddddddddddddddddddd
            repository.WriteLine("using (IDbConnection db = GetIDbConnection())");
            repository.WriteLine("{");

            qmain = "update " + TABLE + " set " + editstr.Substring(1) + " " + editstrwhere;
            //qmain += ")values(" + insertstrvalue.Substring(1) + ")";
            repository.WriteLine("string q =\" " + qmain + "\";");
            repository.WriteLine("db.Execute(q, item);");
            // TODO: saveUserActivity
            //repository.WriteLine("Permission.saveUserActivity(\"update\", iduser, \"" + TABLE + "\", out Message, item." + IDField + ");");

            repository.WriteLine("}");
            repository.WriteLine("}");
        }

        private static void writeInseertPart(System.IO.StreamWriter repository, string TABLE, string insertstr, string insertstrvalue, string IDField)
        {
            string qmain;
            string Lst_Table = TABLE + "_lst";

            repository.WriteLine("public static void " + TABLE + "_insert(" + TABLE + " item)");
            repository.WriteLine("{");
            repository.WriteLine("string Message;");
            repository.WriteLine("using (IDbConnection db = GetIDbConnection())");
            repository.WriteLine("{");

            qmain = "insert into " + TABLE + "( " + insertstr.Substring(1) + "";
            qmain += ")values(" + insertstrvalue.Substring(1) + ");";
            if (IDField != "")
                qmain += "SELECT LAST_INSERT_ID();";

            repository.WriteLine("string q =\" " + qmain + "\";");
            if (IDField != "")
                repository.Write("item." + IDField + "=");
            repository.WriteLine("db.Query<int>(q, item).First ();");

            repository.WriteLine("if (_" + Lst_Table + " != null)");
            repository.WriteLine("_" + Lst_Table + ".Add(item);");

            repository.WriteLine("}");
            repository.WriteLine("}");
        }

        private static void writeInseertPartsqlite(System.IO.StreamWriter repository, string TABLE, string insertstr,
            string insertstrvalue, string IDField)
        {
            string qmain;
            string Lst_Table = TABLE + "_lst";

            repository.WriteLine("public static void " + TABLE + "_insert(" + TABLE + " item)");
            repository.WriteLine("{");
            repository.WriteLine("string Message;");
            repository.WriteLine("using (IDbConnection db = GetIDbConnection())");
            repository.WriteLine("{");

            qmain = "insert into " + TABLE + "( " + insertstr.Substring(1) + "";
            qmain += ")values(" + insertstrvalue.Substring(1) + ");";

            string qInsert = "";
            var qmainByid = "insert into " + TABLE + "( " + IDField + insertstr + "";
            qmainByid += ")values(\"+item." + IDField + "+ \"" + insertstrvalue + ");";

            qInsert += "string q =\" " + qmain + "\";\n";
            qInsert += "if (item." + IDField + ">0)\n";
            qInsert += "q =\" " + qmainByid + "\";";

            string setqlInsertFunc = "getsqlInsert" + TABLE;

            repository.WriteLine("var q = " + setqlInsertFunc + "(item);");
            repository.WriteLine("db.Query(q);");
            // db.Query<int>(q, item).First();
            repository.WriteLine("if (_" + Lst_Table + " != null)");
            repository.WriteLine("_" + Lst_Table + ".Add(item);");

            repository.WriteLine("}");
            repository.WriteLine("}");
            repository.WriteLine("public static string " + setqlInsertFunc + "(" + TABLE + " item){");

            repository.WriteLine(qInsert);

            repository.WriteLine("return q;");
            repository.WriteLine("}");

        }

        private static void setrepositoryLst(System.IO.StreamWriter repository, string TABLE, string IDField)
        {
            string Lst_Table = TABLE + "_lst";
            repository.WriteLine("public static " + TABLE + " " + TABLE + "_Getbyid(int id)");
            repository.WriteLine("{");
            repository.WriteLine("var R = Repository." + Lst_Table + ".Where(p=>p." + IDField + " ==id).FirstOrDefault();");
            repository.WriteLine("return R;");
            repository.WriteLine("}");
        }

        static string TypeMysqlToCStype(string type, ref string defaultv)
        {
            type = type.ToLower().Trim();

            if (type.StartsWith("string"))
            {
                defaultv = " = 0";
                return " String ";
            }
            if (type.StartsWith("bigint"))
            {
                defaultv = " = 0";
                return " Int64 ";
            }
            type = type.ToLower();
            if (type.StartsWith("varchar"))
            {
                defaultv = " = ''";
                return " String ";
            }

            if (type.StartsWith("int"))
            {
                defaultv = " = 0";
                return " int ";
            }
            if (type.StartsWith("text"))
            {
                defaultv = " = ''";
                return " String ";
            }
            if (type.StartsWith("date") | type == "timestamp")
            {
                defaultv = "";

                return " DateTime ";
            }
            if (type.Contains("int("))
            {
                defaultv = " = 0";
                return " int ";
            }

            if (type.StartsWith("decimal"))
            {
                defaultv = " = 0";
                return " decimal ";
            }

            if (type.StartsWith("float"))
            {
                defaultv = " = 0";
                return " float ";
            }
            if (type.StartsWith("double"))
            {
                defaultv = " = 0";
                return " double  ";
            }


            System.Windows.Forms.MessageBox.Show("error in type" + type);


            return "";
        }



        static string TypeSqlitetype(string type)
        {
            type = type.ToLower().Trim();

            if (type.StartsWith("string"))
            {
                return " TEXT ";
            }
            if (type.StartsWith("bigint"))
            {

                return " Int64 ";
            }
            type = type.ToLower();
            if (type.StartsWith("varchar"))
            {
                return type;
            }

            if (type.StartsWith("int"))
            {
                return " int ";
            }
            if (type.StartsWith("text") | type == "longtext")
            {
                return " text ";
            }
            if (type.StartsWith("datetime") | type == "timestamp")
            {
                return " DATETIME ";
            }
            if (type.StartsWith("date"))
            {

                return " date ";
            }
            if (type.Contains("int("))
            {

                return " int ";
            }

            if (type.StartsWith(" decimal "))
            {

                return " DECIMAL ";
            }

            if (type.StartsWith("float"))
            {

                return " DECIMAL ";
            }
            if (type.StartsWith("double"))
            {

                return " DECIMAL ";
            }


            System.Windows.Forms.MessageBox.Show("error in type" + type);


            return "";
        }

    }
}
