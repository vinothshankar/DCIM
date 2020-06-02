insert into RackUnit

values(
      (select max(Id)+1 from RackUnit),1,(select min(FromUnit)-1 from RackUnit),(select min(FromUnit)-1 from RackUnit),
      (select top 1 XCoordinate from RackUnit),(select MAX(YCoordinate)+93 from RackUnit),
      (select top 1 RectangleXCoordinate from RackUnit) ,(select MAX(RectangleYCoordinate)+93 from RackUnit)
     )


DECLARE @numRows int,@i int
SET @numRows = 10
SET @i=1
WHILE @i<=@numRows
 begin
	Declare @ServerId INT = @i;
	Declare @ServerName VARCHAR(10) = ''+@i+'';
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot1')),'SR'+@ServerName+'-S1-P1',47,23,128,50,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot2')),'SR'+@ServerName+'-S2-P2',47,99,129,47,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot3')),'SR'+@ServerName+'-S3-P3',551,34,246,52,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot4')),'SR'+@ServerName+'-S4-P4',412,81,78,81,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot5')),'SR'+@ServerName+'-S5-P5',858,37,331,50,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot6')),'SR'+@ServerName+'-S6-P6',1287,34,194,105,'')
	insert into ServerPortDtl values(@ServerId,(select Id from ServerSlotDtl where ServerId=@ServerId and SlotName =('Slot7')),'SR'+@ServerName+'-S7-P7',254,96,156,65,'')
	SET @i=@i+1
END

insert into RackUnit (RackId,FromUnit,ToUnit,XCoordinate,YCoordinate,RectangleXCoordinate,RectangleYCoordinate) 
select 4,FromUnit,ToUnit,XCoordinate,YCoordinate,RectangleXCoordinate,RectangleYCoordinate from RackUnit where RackId =1

truncate table serverportdtl