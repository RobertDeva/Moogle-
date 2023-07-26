#!/bin/bash
# Funcion destinada a borrar los archivos temporales que se crean durante la compilacion de un latex

   RemoveLatexTemp(){
	   for file in `find $1 -maxdepth 1 ! -name '*.tex'`
	   do
		   rm $file 2> /dev/null
	   done
   }

   RemoveProyectTemp(){
	   rm -r $1/bin/ 2> /dev/null
	   rm -r $1/bin/ 2> /dev/null
   }

   ComprobarComando(){
	  path=$(which $1)

	  if [ "$path" != "" ]
          then
		  return 0
          else
		  return 1
	  fi
  }

  CompilarPDF(){
	  if [ "2$" = "" ]
          then
		  if [ $(ComprobarComando pdflatex)$? -eq 0 ]
		  then
			  echo "Compilando $1 a través de pdfltex ..."
		  else
			  echo -e "\e[31mError: El comando pdflatex no está instalado\e[0m"

		  fi
	  else
		  if [ $(ComprobarComando $2)$? -eq 0 ]
		  then
			  echo "Compilando $1 a través de $2 ..."
		  else
			  echo -e "\e[31mError: El comando $2 no está instalado\e[0m"
		  fi
	  fi
  }


  AbriPDF(){
	  if [ "$2" = "" ]
	  then
		  xdg-open $1
	  else
		  if [ $ComprobarComando $2)$? -eq 0 ]
	          then
			  $2 $1
		  else
			  echo -e "\e[31mError: El comando $2 no está instalado\e[0m"
		  fi
	  fi
  }

  MostrarComandos(){ 
	  echo "Comandos:"
	  echo " $0                \"Mostrar el menu\""
	  echo " $0 run            \"Ejecutar el proyecto\""
	  echo " $0 report         \"Compilar y generar el pdf del latex relativo al informe con pdflatex\""
	  echo " $0 report [comando] \"Compilar y generar el pdf del latex relativo al informe con el comando especificado\""
	  echo " $0 slides         \Compilar y generar el pdf del latex relativo a la presentacion con pdflatex\""
	  echo " $0 slides [comando] \"Compilar y generar el pdf del latex relativo  al la presentacion con el comando especificado\""
	  echo " $0 show_report    \"Visualizar el pdf del informe con el lector de pdf predeterminado\""
	  echo " $0 show_report [comando]  \"Visualizar el pdf del informe con el comando especificado\""
	  echo " $0 show_slides    \"Visualizar el pdf de la presentacion con el lector de pdf predeterminado\""
	  echo " $0 show_slides [comando]  \"Visualizar el pdf de la presentacion con el comando especificado\""
	  echo " $0 clean          \"Eliminar los archivoos temporales de la compilacion del proyecto, el informe y la  presentacion\""
	  echo ""
  }
  

  Evaluar(){
	  case $1 in 
		  run) 
			  xdg-open http://localhost:5000/ > /dev/null 2> /dev/null
		          dotnet watch run --project ../MoogleServer
			  ;;
		  report)
		          cd ../Informe
		          CompilarPDF Informe.tex $2
			  ;;
		  slides) 
		          cd ../Presentación
		          CompilarPDF Presentación.tex $2
			  ;;
		  show_report)
		          if ! [ -e ../Informe/Informe.pdf]
			  then
			  Evaluar report
			  fi
		          AbrirPDF ../Informe/Informe.pdf $2
			  ;;
		 show_slides)
	                  if ! [ -e ../Presentación/Presentación.pdf ]
		          then
		          Evaluar slides
			  fi
	                  AbrirPDF ../Presentación/Presentación.pdf $2
			  ;;
	         clean)  
                          RemoveLatexTemp ../Informe
                          RemoveLatexTemp ../Presentación
                          RemoveLatexTemp ../Presentación/sections
                          RemoveProyectTemp ../MoogleServer
                          RemoveProyectTemp ../MoogleEngine
			  ;;
                 
                 *)
                          echo -e "\e[31mError: Argumento invalido\e[0m"
			  ;;
	  esac
  }

  MostrarOpciones(){
	  echo "Opciones"
	  echo "1) \"Ejecutar el Proyecto\""
	  echo "2) \"Compilar y generar el pdf del latex relativo al informe\""
	  echo "3) \"Compilar y generar el pdf del latex relativo a la presentacionn\""
	  echo "4) \"Visualizar el pdf del informe\""
	  echo "5) \"Visualizar el pdf de la presentacion\""
	  echo "6) \"Eliminar archivos temporales\""
	  echo "7) \"Utilizar la linea de comandos\""
	  echo "8) \"Info\""
	  echo "9) \"Salir\""
	  echo ""
  }

  MostrarInfo(){
	 echo"_________________________________________________________________"

	 echo"|Proyecto desarrollado por Roberto Manuel Martínez Nápoles      |"

	 echo"|1er año de Ciencias de la Computación                          |"

	 echo"|Facultad de Matemática y Computación                           |"

	 echo"|Universidad de La Habana                                       |"

	 echo"_________________________________________________________________"

  }
 
 
  Inicio(){
	 clear
	  
         printf "%*s\n" $(( ($(tput cols) - ${#TEXTO}) / 2)) "Proyecto Moogle"
	 echo ""

	 MostrarOpciones

	 echo -n "Entre la opcion"
	 read -n 1 opcion
	 echo ""
         echo ""

	 case $opcion in 
		 "1")
			 echo "Ejecutando el proyecto"
			 Evaluar run
			 Inicio
			 ;;
	         "2")
			 Evaluar report
			 Inicio
			 ;;
		 "3")
			 Evaluar slides
			 Inicio
			 ;;
		 "4") 
			 Evaluar show_report
			 Inicio
			 ;;
		 "5")
			 Evaluar show_slides
			 Inicio
			 ;;
	         "6")
			 Evaluar clean
			 echo "Los archivos temporales fueron eliminados"
			 Inicio
			 ;;
		 "7")    MostarComandos
			 exit 0
			 ;;
		 "8")
			 MostrarInfo
			 echo ""
			 exit 0
			 ;;
		 "9")
			 exit 0
			 ;;
		  *) 
			  #echo e-"e[31mError: Opcion Invalida\e[0m"
			  #sleep 1
			  Inicio
			  ;;
	  esac

 }


 ComprobarArgumentos(){
	 if [ $# -eq 0 ]
	 then
		 Inicio
	 elif [ $# -ne 1] && ! [[ ( "$1" = "slides" || "$1" = "report" || "$1" = "shoe_slides" || "$1" = "show_report") && $# -eq 2 ]]
	 then
		 echo -e "\e[31mError: El numero de argumentos es incorrecto\e[0m"
	 else
		 Evaluar $1 $2
	 fi
 }

  ComprobarArgumentos $*










