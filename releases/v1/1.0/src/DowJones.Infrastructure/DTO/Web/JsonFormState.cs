using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using DowJones.Tools.Session;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.DTO.Web.JSON;
using DowJones.Utilities.DTO.Web.LOB;
using DowJones.Utilities.DTO.Web.Request;
using Simplicit.Net.Lzo;

namespace DowJones.Utilities.DTO.Web
{
	public sealed class FormState : AbstractFormState
	{
		private readonly JSONObject _currentState;
		private JSONObject _previousState;
		private bool _registerEventHandler = true;
		private readonly HttpContext _httpContext = HttpContext.Current;
       
		public static string FORM_KEY_STATE = "_XFORMSTATE";
		public static string FORM_KEY_SESS_STATE = "_XFORMSESSSTATE";

		public FormState()
		{
			Load();
			_currentState = new JSONObject();
		}

		public FormState(string encodedStr)
		{
			Load(encodedStr);
			_currentState = new JSONObject();
		}

        public override string EmptyDtoFormStateString
		{
			get { return GetSessionData(); }
		}

		public override IRequestDTO Accept(Type type, bool persist)
		{
			ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
			object obj = constructorInfo.Invoke(null);
			IRequestDTO dto = (IRequestDTO) obj;
			LoadObjectState(dto);
			dto.Load();

			if (persist)
			{
				Add(dto);
			}

			return dto;
		}

		public override void Remove(IRequestDTO dto)
		{
            JSONObject subTable = EnsureRootKey(dto);
            subTable.remove(dto.Key);
		}

		public override void Add(IRequestDTO dto)
		{
			JSONObject subTable = EnsureRootKey(dto);
			JSONObject translatedLob = JsonFormStateAssembler.GetJsonObject(dto);
			if (subTable.has(dto.Key))
				throw new NotSupportedException(String.Format("Multiple key not allowed for {0}", dto.GetType().FullName));
			subTable.put(dto.Key, translatedLob);
			RegisterEventHandler();
		}

		protected override void Load(string formState)
		{
			_previousState = new JSONObject();
			if (formState != null && formState.Trim().Length > 0)
			{
				formState = ApplyFilter(formState, false);
				_previousState = new JSONObject(formState);
			}
		}

		protected override void Load()
		{
			string formState = _httpContext.Request[FORM_KEY_STATE];
			_previousState = new JSONObject();
			if (formState != null && formState.Trim().Length > 0)
			{
				formState = ApplyFilter(formState, false);
				_previousState = new JSONObject(formState);
			}
			string sessState = _httpContext.Request[FORM_KEY_SESS_STATE];
		    if (sessState == null || sessState.Trim().Length <= 0)
		        return;
		    sessState = ApplyFilter(sessState, false);
		    JSONObject sessionJsonObject = new JSONObject(sessState);
		    string tempKey = GetRootKey(DtoPersistance.Session);
		    if (!_previousState.has(tempKey) && sessionJsonObject.has(tempKey))
		    {
		        _previousState.put(tempKey, sessionJsonObject.getJSONObject(tempKey));
		    }
		    tempKey = GetRootKey(DtoPersistance.Breadtrail);
		    if (!_previousState.has(tempKey) && sessionJsonObject.has(tempKey))
		    {
		        _previousState.put(tempKey, sessionJsonObject.getJSONObject(tempKey));
		    }
		    tempKey = GetRootKey(DtoPersistance.Persisted);
		    if (!_previousState.has(tempKey) && sessionJsonObject.has(tempKey))
		    {
		        _previousState.put(tempKey, sessionJsonObject.getJSONArray(tempKey));
		    }
		}

        public override string GetFormState()
        {
            object objSession = _currentState.remove(GetRootKey(DtoPersistance.Session));
            object objBreadTrail = _currentState.remove(GetRootKey(DtoPersistance.Breadtrail));
            object objPersited = _currentState.remove(GetRootKey(DtoPersistance.Persisted));
            string formState = _currentState.ToString();
            _currentState.put(GetRootKey(DtoPersistance.Session), objSession);
            _currentState.put(GetRootKey(DtoPersistance.Breadtrail), objBreadTrail);
            _currentState.put(GetRootKey(DtoPersistance.Persisted), objPersited);
            if (formState != null && formState.Trim().Length > 0)
            {
                formState = ApplyFilter(formState, true);
            }
            return formState;
        }

		protected override void Save()
		{
			Page page = _httpContext.Handler as Page;
			if (page != null)
			{
				page.ClientScript.RegisterHiddenField(FORM_KEY_SESS_STATE, GetSessionData() );
                page.ClientScript.RegisterHiddenField(FORM_KEY_STATE, GetFormState());
			}
		}

		public static string GetFormStateForUrl(IRequestDTO dto)
		{
			JSONObject state = new JSONObject();

			SessionRequestDTO sessionRequestDTO = new SessionRequestDTO();
			sessionRequestDTO.AccessPointCode = SessionData.Instance().AccessPointCode;
			sessionRequestDTO.InterfaceLanguage = SessionData.Instance().InterfaceLanguage;

			JSONObject subTable = EnsureRootKey(state, sessionRequestDTO.RootKey);
			subTable.put(sessionRequestDTO.Key, JsonFormStateAssembler.GetJsonObject(sessionRequestDTO));

			if (dto.RootKey != DtoPersistance.Session)
			{
				subTable = EnsureRootKey(state, dto.RootKey);
				subTable.put(dto.Key, JsonFormStateAssembler.GetJsonObject(dto));
			}

			string formStateString = state.ToString();
			formStateString = ApplyFilter(formStateString, true);
			return String.Format("{0}={1}", FORM_KEY_STATE, HttpUtility.UrlEncode(formStateString));
		}
		
		public static string GetFormState(IRequestDTO dto)
		{
			JSONObject state = new JSONObject();

			SessionRequestDTO sessionRequestDTO = new SessionRequestDTO();
			sessionRequestDTO.AccessPointCode = SessionData.Instance().AccessPointCode;
			sessionRequestDTO.InterfaceLanguage = SessionData.Instance().InterfaceLanguage;

			JSONObject subTable = EnsureRootKey(state, sessionRequestDTO.RootKey);
			subTable.put(sessionRequestDTO.Key, JsonFormStateAssembler.GetJsonObject(sessionRequestDTO));

			if (dto.RootKey != DtoPersistance.Session)
			{
				subTable = EnsureRootKey(state, dto.RootKey);
				subTable.put(dto.Key, JsonFormStateAssembler.GetJsonObject(dto));
			}

			string formStateString = state.ToString();
			return ApplyFilter(formStateString, true);
		}

		public override string GetSessionData()
		{
			return ApplyFilter(GetSessionNVP(_currentState).ToString(), true);
		}

		private static JSONObject GetSessionNVP(JSONObject jsonObject)
		{
			JSONObject state = new JSONObject();
			string rootKey = GetRootKey(DtoPersistance.Session);
			if (jsonObject.has(rootKey))
				state.put(rootKey, jsonObject.getJSONObject(rootKey));
			rootKey = GetRootKey(DtoPersistance.Breadtrail);
			if (jsonObject.has(rootKey))
				state.put(rootKey, jsonObject.getJSONObject(rootKey));
			rootKey = GetRootKey(DtoPersistance.Persisted);
			if (jsonObject.has(rootKey))
				state.put(rootKey, jsonObject.getJSONArray(rootKey));
			return state;
		}

		private void LoadObjectState(IRequestDTO source)
		{
		    Object[] objField = source.GetType().GetFields();
            if (objField.Length <= 0)
                objField = source.GetType().GetProperties();

            //FieldInfo[] fields = source.GetType().GetFields();
			for (int i = 0; i < objField.Length; i++)
			{
				
				//FieldInfo field = fields[i];
                object ofield = objField[i];
                
			    ParameterName parameterName;
                if (ofield is FieldInfo)
                {
                    FieldInfo field;    
                    field = ofield as FieldInfo;
                    parameterName = (ParameterName)Attribute.GetCustomAttribute(field, typeof(ParameterName));
                    //UsesCompression usesCompression = (UsesCompression)UsesCompression.GetCustomAttribute(field, typeof(UsesCompression));
                    if (parameterName == null)
                        continue;
                    object fieldValue;
                    string requestValue = _httpContext.Request[parameterName.Value];
                    if (requestValue != null && field.FieldType == typeof(string))
                    {
                        fieldValue = JsonFormStateAssembler.WriteDomainObject(requestValue.Trim(), field.FieldType);
                    }
                    else if (requestValue != null && requestValue.Trim() != string.Empty)
                    {
                        fieldValue = JsonFormStateAssembler.WriteDomainObject(requestValue.Trim(), field.FieldType);
                    }
                    else
                    {
                        // Changed from using integers to using the parameter name D.D 3/15/2007
                        fieldValue = WriteDomainObjectFromPreviousState(source, parameterName.Value, field.FieldType) ?? field.GetValue(source);
                    }
                    field.SetValue(source, fieldValue);
                }
                else if (ofield is PropertyInfo)
                {
                    PropertyInfo pfield = ofield as PropertyInfo;
                    parameterName = (ParameterName)Attribute.GetCustomAttribute(pfield, typeof(ParameterName));
                    //UsesCompression usesCompression = (UsesCompression)UsesCompression.GetCustomAttribute(field, typeof(UsesCompression));
                    if (parameterName == null)
                        continue;
                    object fieldValue;
                    string requestValue = _httpContext.Request[parameterName.Value];
                    if (requestValue != null && pfield.PropertyType == typeof(string))
                    {
                        fieldValue = JsonFormStateAssembler.WriteDomainObject(requestValue.Trim(), pfield.PropertyType);
                    }
                    else if (requestValue != null && requestValue.Trim() != string.Empty)
                    {
                        fieldValue = JsonFormStateAssembler.WriteDomainObject(requestValue.Trim(), pfield.PropertyType);
                    }
                    else
                    {
                        // Changed from using integers to using the parameter name D.D 3/15/2007
                        fieldValue = WriteDomainObjectFromPreviousState(source, parameterName.Value, pfield.PropertyType) ?? pfield.GetValue(source,null);
                    }
                    pfield.SetValue(source, fieldValue,null);
                }
			    
			}
			//return source;
		}

		private object WriteDomainObjectFromPreviousState(IRequestDTO source, string fieldName, Type type)
		{
			return WriteDomainObjectFromXState(_previousState, source, fieldName, type);
		}

		private static object WriteDomainObjectFromXState(JSONObject jsonObject, IRequestDTO source, string fieldName, Type type)
		{
			if (jsonObject.has(GetRootKey(source.RootKey)))
			{
				JSONObject obj = jsonObject.getJSONObject(GetRootKey(source.RootKey));
				if (obj.has(source.Key))
				{
					obj = obj.getJSONObject(source.Key);
					if (obj.has(fieldName))
						return WriteDomainObjectFromState(obj.getValue(fieldName), type);
				}
			}
			return null;
		}

		private static object WriteDomainObjectFromState(object stateValue, Type type)
		{
		    if (stateValue.GetType() == typeof (JSONObject))
				return JsonFormStateAssembler.WriteDomainObject((JSONObject) stateValue, type);
		    return stateValue.GetType() == typeof (JSONArray) ? JsonFormStateAssembler.WriteDomainObject((JSONArray) stateValue, type) : JsonFormStateAssembler.WriteDomainObject(stateValue.ToString(), type);
		}

	    public override string GetState(object domainObject)
		{
			return GetState(domainObject, false);
		}

		public override string GetState(object domainObject, bool escapeForJavascript)
		{
		    Type objType = domainObject.GetType();

			string str = objType.IsArray ? (JsonFormStateAssembler.GetJsonArray(domainObject).ToString()) : (JsonFormStateAssembler.GetJsonObject(domainObject).ToString());

			if (escapeForJavascript)
				str = str.Replace("\"", "&quot;").Replace("'", "&apos;");

			return str;
		}

		private static string GetRootKey(DtoPersistance persistance)
		{
			return ((int) persistance).ToString();
		}

		private JSONObject EnsureRootKey(IRequestDTO dto)
		{
			return EnsureRootKey(_currentState, dto.RootKey);
		}

		private static JSONObject EnsureRootKey(JSONObject jsonObject, DtoPersistance dtoPersistance)
		{
			string key = GetRootKey(dtoPersistance);
			if (!jsonObject.has(key))
				jsonObject.put(key, new JSONObject());
			return jsonObject[key] as JSONObject;
		}

		private void RegisterEventHandler()
		{
		    if (!_registerEventHandler)
		        return;
		    ((Page) _httpContext.Handler).PreRender += JsonFormState_PreRender;
		    _registerEventHandler = false;
		}

		private void JsonFormState_PreRender(object sender, EventArgs e)
		{
			//if (UserSessionData.Instance().SaveFormState)
			Save();
		}

		private static string ApplyFilter(string str, bool zip)
		{
		    Debug.WriteLine(new string('*', 80));
		    Debug.WriteLine(str);
		    LZOCompressor lzo = new LZOCompressor();
		    UTF8Encoding utfEncoder = new UTF8Encoding();
		    try
		    {
		        if (zip)
		        {
		            byte[] byteA = utfEncoder.GetBytes(str);
		            byte[] byteB = lzo.Compress(byteA);
		            string compStr = Convert.ToBase64String(byteB);
		            Debug.WriteLine(compStr);
		            Debug.WriteLine(new string('*', 80));
		            return compStr;
		        }
		        else
		        {
		            byte[] byteB = Convert.FromBase64String(str);
		            byte[] byteA = lzo.Decompress(byteB);
		            string temp = utfEncoder.GetString(byteA);
		            Debug.WriteLine(temp);
		            Debug.WriteLine(new string('*', 80));
		            return temp;
		        }
		    }
		    catch (Exception ex)
		    {
		        Debug.WriteLine(ex.ToString());
		    }
		    throw new Exception(String.Format("Failed to {0}compress data.", (zip) ? "" : "un"));
		}
	}
}