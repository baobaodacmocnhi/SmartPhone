SELECT ID_HOADON AS IDkey, hd.DANHBA, hd.TENKH, SO as SONHA,DUONG as TENDUONG,hd.GB,hd.DM, hd.KY as KyHD, hd.NAM as NamHD, hd.PHI as PBVMT, hd.THUE as TGTGT,hd.GIABAN as TNuoc,hd.TONGCONG as TONGCONG 
FROM HOADON hd
 WHERE NGAYGIAITRACH IS NULL AND hd.DANHBA='13132180072'
 AND hd.DANHBA NOT IN (SELECT DANHBA FROM BGW_HOADON WHERE BGW_HOADON.KY=hd.KY and BGW_HOADON.NAM=hd.NAM )
 AND hd.DANHBA NOT IN (SELECT Dbo FROM ThuOnline WHERE ThuOnline.KyHD=hd.KY and ThuOnline.NamHD=hd.NAM )
 AND hd.DANHBA NOT IN (SELECT DANHBA FROM SimpayDB WHERE SimpayDB.KyHD=hd.KY and SimpayDB.NamHD=hd.NAM )
 AND hd.DANHBA NOT IN (SELECT DANHBA FROM Agribank_THUTAM WHERE Agribank_THUTAM.KyHD=hd.KY and Agribank_THUTAM.NamHD=hd.NAM )
ORDER BY NAM DESC, KY DESC