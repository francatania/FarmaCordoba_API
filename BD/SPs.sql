USE Farmaceutica
GO
CREATE PROCEDURE SP_TOTALES_FACTURADOS_VENDEDORES
@a�o int

AS
BEGIN
	SELECT YEAR(F.FECHA) 'A�O', 
		   MONTH(F.FECHA) 'MES',
		   F.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS 'ID_PERSONAL',
		   P.APELLIDO + ', ' + P.NOMBRE 'PERSONAL',
		   SUM(D.CANTIDAD * D.PRECIO_UNITARIO) 'TOTAL_FACTURADO',
		   MAX(D.CANTIDAD*D.PRECIO_UNITARIO) 'VENTA_MAS_CARA'
	FROM FACTURAS F
		JOIN PERSONAL_CARGOS_ESTABLECIMIENTOS PCE ON PCE.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS =  F.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
		JOIN DISPENSACIONES D ON D.ID_FACTURA = F.ID_FACTURA
		JOIN PERSONAL P ON P.ID_PERSONAL = PCE.ID_PERSONAL
	WHERE YEAR(F.FECHA) = @a�o
	GROUP BY YEAR(F.FECHA), MONTH(F.FECHA), P.APELLIDO + ', ' + P.NOMBRE, F.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
	HAVING SUM(D.CANTIDAD * D.PRECIO_UNITARIO) > (SELECT SBC.PROMEDIO
														FROM (SELECT F1.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS 'ID_PERSONAL_1', AVG(D1.CANTIDAD*(D1.PRECIO_UNITARIO-(D1.DESCUENTO*D1.PRECIO_UNITARIO))) 'PROMEDIO'
															  FROM FACTURAS F1
															  JOIN DISPENSACIONES D1 ON D1.ID_FACTURA = F1.ID_FACTURA
															  WHERE YEAR(F1.FECHA) = YEAR(GETDATE())
															  GROUP BY F1.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
															  ) AS SBC
														WHERE SBC.ID_PERSONAL_1 = F.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS)
END

 
GO
CREATE PROCEDURE SP_TOTALES_FACTURADOS_FARMACIAS
@a�o INT = NULL
AS
BEGIN
	IF(@a�o IS NULL)
		BEGIN
			select E.ID_ESTABLECIMIENTO,e.NOMBRE 'Establecimiento',  CAST(SUM(d.CANTIDAD * (d.PRECIO_UNITARIO - (d.PRECIO_UNITARIO * d.DESCUENTO))) AS DECIMAL(10,2)) AS 'Total facturado'

				,(select '$' + FORMAT(SCMA.Facturado, 'N2') + ', a�o ' + CAST(SCMA.A�o AS VARCHAR(5)) from

						(select top 1 year(f.FECHA) 'A�o' , e1.NOMBRE, SUM(d.CANTIDAD * (d.PRECIO_UNITARIO - (d.PRECIO_UNITARIO * d.DESCUENTO))) 'Facturado'  from FACTURAS f 	
							join DISPENSACIONES d on f.ID_FACTURA = d.ID_FACTURA
							join PERSONAL_CARGOS_ESTABLECIMIENTOS pce on f.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS = pce.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
							join ESTABLECIMIENTOS e1 on pce.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
							where e1.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
								and d.ID_MEDICAMENTO_LOTE is not null
							group by year(f.FECHA), e1.NOMBRE		
							order by 3 desc) as SCMA) 'Mejor a�o facturado' from FACTURAS f

				join DISPENSACIONES d on f.ID_FACTURA = d.ID_FACTURA
				join PERSONAL_CARGOS_ESTABLECIMIENTOS pce on f.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS = pce.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
				join ESTABLECIMIENTOS e on pce.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
				where d.ID_MEDICAMENTO_LOTE is not null
				group by  E.ID_ESTABLECIMIENTO, e.NOMBRE
		END			
	ELSE
		BEGIN
			select E.ID_ESTABLECIMIENTO,e.NOMBRE 'Establecimiento', CAST(SUM(d.CANTIDAD * (d.PRECIO_UNITARIO - (d.PRECIO_UNITARIO * d.DESCUENTO))) AS DECIMAL(10,2)) AS 'Total facturado'

				,(select '$' + FORMAT(SCMA.Facturado, 'N2') + ', a�o ' + CAST(SCMA.A�o AS VARCHAR(5)) from

						(select top 1 year(f.FECHA) 'A�o' , e1.NOMBRE, SUM(d.CANTIDAD * (d.PRECIO_UNITARIO - (d.PRECIO_UNITARIO * d.DESCUENTO))) 'Facturado'  from FACTURAS f 	
							join DISPENSACIONES d on f.ID_FACTURA = d.ID_FACTURA
							join PERSONAL_CARGOS_ESTABLECIMIENTOS pce on f.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS = pce.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
							join ESTABLECIMIENTOS e1 on pce.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
							where e1.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
								and d.ID_MEDICAMENTO_LOTE is not null
							group by year(f.FECHA), e1.NOMBRE		
							order by 3 desc) as SCMA) 'Mejor a�o facturado' from FACTURAS f

				join DISPENSACIONES d on f.ID_FACTURA = d.ID_FACTURA
				join PERSONAL_CARGOS_ESTABLECIMIENTOS pce on f.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS = pce.ID_PERSONAL_CARGOS_ESTABLECIMIENTOS
				join ESTABLECIMIENTOS e on pce.ID_ESTABLECIMIENTO = e.ID_ESTABLECIMIENTO
				where d.ID_MEDICAMENTO_LOTE is not null and YEAR(f.FECHA) = @a�o
				group by  E.ID_ESTABLECIMIENTO, e.NOMBRE
		END
END


GO


CREATE PROCEDURE SP_MEDICAMENTOS_BAJO_MOVIMIENTOS
@MES INT = 01,
@A�O INT = 2024,
@PRESENTACION VARCHAR(50) = '%'
AS
	BEGIN
		SELECT SBC.MEDICAMENTO, SBC.DESCRIPCION, SBC.PRECIO
		FROM (SELECT M.NOMBRE_COMERCIAL 'MEDICAMENTO', M.DESCRIPCION 'DESCRIPCION', M.PRECIO 'PRECIO'
			  FROM MEDICAMENTOS M
			  JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
			  JOIN DISPENSACIONES D ON D.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
			  JOIN FACTURAS F ON F.ID_FACTURA = D.ID_FACTURA
			  JOIN PRESENTACIONES P ON P.ID_PRESENTACION = M.ID_PRESENTACION
			  WHERE ML.ID_MEDICAMENTO_LOTE != ALL (SELECT SBC.ID_MEDICAMENTO_LOTE FROM(SELECT M.ID_MEDICAMENTO 'ID_MEDICAMENTO', ML.ID_MEDICAMENTO_LOTE 'ID_MEDICAMENTO_LOTE'
			  									 FROM MEDICAMENTOS M
			  									 JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
			  									 JOIN DETALLES_PEDIDOS DP ON DP.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
			  									 JOIN PEDIDOS P ON P.ID_PEDIDO = DP.ID_PEDIDO
			  									 WHERE DP.ID_MEDICAMENTO_LOTE IS NOT NULL
			  									 AND MONTH(P.FECHA) = @MES AND @A�O = YEAR(P.FECHA)) SBC)
			  AND P.PRESENTACION NOT LIKE '%' + @PRESENTACION + '%'
			  AND ML.ID_MEDICAMENTO_LOTE NOT IN (SELECT ML.ID_MEDICAMENTO_LOTE 'ID_MEDICAMENTO_LOTE'
			  									 FROM MEDICAMENTOS M
			  									 JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
			  									 JOIN DISPENSACIONES D ON D.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
			  									 JOIN FACTURAS F ON F.ID_FACTURA = D.ID_FACTURA
			  									 WHERE D.ID_MEDICAMENTO_LOTE IS NOT NULL
			  									 AND MONTH(F.FECHA) = @MES AND YEAR(GETDATE()) = @A�O)
			  GROUP BY M.NOMBRE_COMERCIAL, M.DESCRIPCION, M.PRECIO
			  HAVING M.PRECIO > (SELECT AVG(M1.PRECIO)
			  				   FROM MEDICAMENTOS M1)) SBC
		WHERE SBC.PRECIO > (SELECT AVG(SBC1.PRECIO)
							FROM (SELECT M.NOMBRE_COMERCIAL 'MEDICAMENTO', M.DESCRIPCION 'DESCRIPCION', M.PRECIO
								  FROM MEDICAMENTOS M
								  JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
								  JOIN DISPENSACIONES D ON D.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
								  JOIN FACTURAS F ON F.ID_FACTURA = D.ID_FACTURA
								  JOIN PRESENTACIONES P ON P.ID_PRESENTACION = M.ID_PRESENTACION
								  WHERE ML.ID_MEDICAMENTO_LOTE != ALL (SELECT SBC.ID_MEDICAMENTO_LOTE FROM(SELECT M.ID_MEDICAMENTO 'ID_MEDICAMENTO', ML.ID_MEDICAMENTO_LOTE 'ID_MEDICAMENTO_LOTE'
								  									 FROM MEDICAMENTOS M
								  									 JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
								  									 JOIN DETALLES_PEDIDOS DP ON DP.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
								  									 JOIN PEDIDOS P ON P.ID_PEDIDO = DP.ID_PEDIDO
								  									 WHERE DP.ID_MEDICAMENTO_LOTE IS NOT NULL
								  									 AND MONTH(P.FECHA) = @MES AND YEAR(GETDATE()) = @A�O) SBC)
								  AND P.PRESENTACION NOT LIKE '%' + @PRESENTACION + '%'
								  AND ML.ID_MEDICAMENTO_LOTE NOT IN (SELECT ML.ID_MEDICAMENTO_LOTE 'ID_MEDICAMENTO_LOTE'
								  									 FROM MEDICAMENTOS M
								  									 JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO = M.ID_MEDICAMENTO
								  									 JOIN DISPENSACIONES D ON D.ID_MEDICAMENTO_LOTE = ML.ID_MEDICAMENTO_LOTE
								  									 JOIN FACTURAS F ON F.ID_FACTURA = D.ID_FACTURA
								  									 WHERE D.ID_MEDICAMENTO_LOTE IS NOT NULL
								  									 AND MONTH(F.FECHA) = @MES AND YEAR(GETDATE()) = @A�O)
								  GROUP BY M.NOMBRE_COMERCIAL, M.DESCRIPCION, M.PRECIO
								  HAVING M.PRECIO > (SELECT AVG(M1.PRECIO)
								  				   FROM MEDICAMENTOS M1)) SBC1)
	END

GO


CREATE PROCEDURE SP_MAYORES_COMPRAS
@A�O INT = 2024,
@CONTACTO VARCHAR = '%',
@CANTIDAD INT = 1
AS
	BEGIN
		SELECT 
		    YEAR(F.FECHA) 'A�o',  
		    DATENAME(MONTH, F.FECHA) 'Mes', 
		    C.NOMBRE + ' ' + C.APELLIDO 'Cliente', 
		    SUM(CASE WHEN D.ID_MEDICAMENTO_LOTE IS NOT NULL THEN D.CANTIDAD ELSE 0 END) 'Total_Medicamentos',
		    CO.CONTACTO 'Contacto', 
		    B.BARRIO 'Barrio', 
		    COUNT(*) 'Cantidad_Compras', 
		    MAX(D.CANTIDAD * (D.PRECIO_UNITARIO - (D.PRECIO_UNITARIO * D.DESCUENTO))) 'Mayor_Monto' 
		FROM 
		    DISPENSACIONES D
		JOIN FACTURAS F ON F.ID_FACTURA = D.ID_FACTURA
		JOIN CLIENTES C ON F.ID_CLIENTE = C.ID_CLIENTE
		JOIN CONTACTOS CO ON CO.ID_CLIENTE = C.ID_CLIENTE
		JOIN BARRIOS B ON B.ID_BARRIO = C.ID_BARRIO
		JOIN MEDICAMENTOS_LOTES ML ON ML.ID_MEDICAMENTO_LOTE = D.ID_MEDICAMENTO_LOTE
		JOIN MEDICAMENTOS M ON M.ID_MEDICAMENTO = ML.ID_MEDICAMENTO
		JOIN TIPOS_CONTACTOS TP ON CO.ID_TIPO_CONTACTO = TP.ID_TIPO_CONTACTO
		
		WHERE TP.TIPO_CONTACTO LIKE '%' + @CONTACTO + '%'
		AND YEAR(F.FECHA) = @A�O
		
		GROUP BY 
		    YEAR(F.FECHA), 
		    DATENAME(MONTH, F.FECHA), 
			C.ID_CLIENTE,
		    C.NOMBRE + ' ' + C.APELLIDO, 
		    CO.CONTACTO, 
		    B.BARRIO
		HAVING 
		    MAX(D.CANTIDAD * (D.PRECIO_UNITARIO - (D.PRECIO_UNITARIO * D.DESCUENTO))) > 
		    (SELECT AVG(D1.CANTIDAD * (D1.PRECIO_UNITARIO - (D1.PRECIO_UNITARIO * D1.DESCUENTO))) 
		     FROM DISPENSACIONES D1) 
			AND COUNT(*) >= @CANTIDAD
		ORDER BY 
		   Total_Medicamentos DESC;
	END

GO


CREATE PROCEDURE SP_REPORTE_MENSUAL_COBERTURA
@ANIO INT,
@MES INT,
@obra_social INT
AS
BEGIN
Select
	TC.DESCRIPCION 'Tipo_de_cobertura',
	SUM( CASE
			WHEN D.ID_COBERTURA IS NOT NULL THEN (d.CANTIDAD * d.PRECIO_UNITARIO * d.DESCUENTO)
			ELSE 0
			END) 'Importe_a_reintegrar'
FROM DISPENSACIONES D
JOIN TIPOS_COBERTURAS TC ON D.ID_COBERTURA = TC.ID_TIPO_COBERTURA
JOIN OBRA_SOCIAL OS ON TC.ID_OBRA_SOCIAL = OS.ID_OBRA_SOCIAL
JOIN Facturas f ON d.ID_FACTURA = f.ID_FACTURA
WHERE YEAR(f.FECHA) = @ANIO
AND MONTH(F.FECHA) = @MES
AND OS.ID_OBRA_SOCIAL = @OBRA_SOCIAL
GROUP BY TC.DESCRIPCION
END


GO




CREATE PROCEDURE SP_REPORTE_MENSUAL_OBRA_SOCIAL
@ANIO INT,
@MES INT,
@obra_social INT
AS
BEGIN
Select OS.NOMBRE 'Obra_Social',
	SUM(CASE
			WHEN D.ID_COBERTURA IS NOT NULL THEN (d.CANTIDAD * d.PRECIO_UNITARIO * d.DESCUENTO)
			ELSE 0
			END) 'Importe_a_reintegrar'
FROM DISPENSACIONES D
JOIN TIPOS_COBERTURAS TC ON D.ID_COBERTURA = TC.ID_TIPO_COBERTURA
JOIN OBRA_SOCIAL OS ON TC.ID_OBRA_SOCIAL = OS.ID_OBRA_SOCIAL
JOIN Facturas f ON d.ID_FACTURA = f.ID_FACTURA
WHERE YEAR(f.FECHA) = @ANIO
AND MONTH(F.FECHA) = @MES
AND OS.ID_OBRA_SOCIAL = @OBRA_SOCIAL
GROUP BY OS.NOMBRE
END
