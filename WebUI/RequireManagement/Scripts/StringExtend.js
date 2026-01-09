/**
 * extending string prototype.
 * @author James Lee
 * @date created 2007-4-29
 * @from CMS David Yan & others
 * @modify by James Lee 2007/6/13
 * 
 * Function List:
 *
 * 1 , String.trim()
 * 
 * 2 , String.isEmpty()
 * 
 * 3 , String.isInsideOf(specifyStr) //to check whether or not only include specify string
 * 
 * 4 , String.isAlpha()
 * 
 * 5 , String.isInteger()
 * 
 * 6 , String.isDouble()
 * 
 * 7 , String.isNumberInPrecision(precision,scale)
 * 
 * 8 , String.isNumberInRange(min,max) //to check the number whether or not between min and max.
 * 
 * 9 , String.isEmail()
 *
 * 10, String.isAlphanumeric()
 *
 * 11, String.replaceAll()
 * 
 * 
 */
 
 String.prototype.ISOLen = function()
 {    
	 var i,str1,str2,str3,nLen;
	 str1 = this;
	 nLen = 0;	 
	 for(i=1;i<=str1.length;i++) {
		str2=str1.substring(i-1,i)
		str3=escape(str2);
		if(str3.length>3){
			nLen = nLen + 2;
		}else {
			nLen = nLen + 1;
		}
	 }
	 return nLen;	
 }
 String.prototype.trim = function()
{
    return this == null ? "" : this.replace(/(^[\s]*)|([\s]*$)/g, "");
}
String.prototype.lTrim = function()
{
    return this == null ? "" : this.replace(/(^[\s]*)/g, "");
}
String.prototype.rTrim = function()
{
    return this == null ? "" : this.replace(/([\s]*$)/g, "");
}
String.prototype.isEmpty = function()
{
    return this == null ? true : this.trim()=="";
}
//to check whether or not only include specify string
String.prototype.isInsideOf = function(specifyStr)
{
	if (this.isEmpty()) return true;
	for (var i = 0; i < this.length; i++){
		if (specifyStr.indexOf(this.charAt(i)) == -1) return false;
	}
	return true;
}
String.prototype.isAlpha = function()
{
	/*var validAlpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	return this.isInsideOf(validAlpha);*/
	return /^[a-zA-Z]+$/g.test(this);
}
String.prototype.isInteger = function()
{
	return /^(\-?)(\d+)$/.test(this);
}
String.prototype.isDouble = function()
{  
    return this.indexOf(".") == -1 ? this.isInteger() : /^(\-?)(\d+)(.{1})(\d+)$/.test(this);
}
String.prototype.isNumberInPrecision = function(precision,scale)
{
    if(this.isEmpty()) return true;
	var integerLen = precision-scale;
	if (this.indexOf(".") == -1){
		//if (!(/^(\-?)(\d+)$/g.test(this))) return false;
		if (!(/^(\-?)(\d+)$/.test(this))) return false;
		if ((RegExp.$2).length>integerLen) return false;
	}else{
		if (scale==0) return false;
		//if (!(/^(\-?)(\d+)(.{1})(\d+)$/g.test(this))) return false;
		if (!(/^(\-?)(\d+)(.{1})(\d+)$/.test(this))) return false;
		var num1=(RegExp.$2).length;
		var num2=(RegExp.$4).length;
		if (num1>integerLen) return false;
		if (num2>scale) return false;
		if (num1+num2>precision) return false;
	}	
	return true;
}
//to check the number whether or not between min and max.
String.prototype.isNumberInRange = function(min,max)
{
    if(!this.isDouble()) return false;
	var num = parseFloat(this);
	return ((num >= min) && (num <= max));
}
String.prototype.isEmail = function()
{
	//return /^([a-zA-Zd_\.\-])+\@(([a-zA-Zd\-])+\.)+([a-zA-Zd]{2,4})+$/g.test(this);
	//return /^[\w-]+@[\w-]+\.(com|net|org|edu|mil|tv|biz|info)$/g.test(this);
	if(this.isEmpty()) return true;
	return /^(['\w]+([-+.]['\w]+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)(;(['\w]+([-+.]['\w]+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*))*$/.test(this);//for multi email address ,split by ';'
}
String.prototype.isAlphanumeric = function()
{
    return /^\w+$/gi.test(this);
}
String.prototype.replaceAll = function(source,target)
{
	var str2="";
	var i1=this.indexOf(source);
	var i2=0;
	for(;i1>=0;i1=this.indexOf(source,i2))
	{
		str2+=this.substring(i2,i1) + target;
		i2=i1+source.length;
	}
	str2+=this.substring(i2);
	return str2;
}